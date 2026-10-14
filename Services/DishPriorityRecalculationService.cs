using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Services;

public class DishPriorityRecalculationService : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(30);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DishPriorityRecalculationService> _logger;

    public DishPriorityRecalculationService(IServiceScopeFactory scopeFactory, ILogger<DishPriorityRecalculationService> logger)
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
                using var scope = _scopeFactory.CreateScope();
                var priorityService = scope.ServiceProvider.GetRequiredService<IDishPriorityService>();
                await priorityService.RecalculatePrioritiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recalculate dish priorities");
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
}
