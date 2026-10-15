using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services;

public class StockExpirationService : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockExpirationService> _logger;

    public StockExpirationService(IServiceScopeFactory scopeFactory, ILogger<StockExpirationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiredStocksAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check expired stocks");
            }

            try
            {
                await Task.Delay(CheckInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    private async Task CheckExpiredStocksAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var stockR = scope.ServiceProvider.GetRequiredService<IStockRepo>();

        var expired = await stockR.GetExpiredActiveAsync();
        if (expired.Count == 0)
        {
            _logger.LogInformation("No expired stocks found");
            return;
        }

        foreach (var stock in expired)
        {
            stock.Status = Stock.StockStatus.Expired;
            await stockR.UpdateAsync(stock);
        }

        _logger.LogInformation("Marked {Count} stocks as expired", expired.Count);
    }
}
