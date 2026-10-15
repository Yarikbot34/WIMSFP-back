using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class StockRepo : IStockRepo
{
    private readonly AppDbContext _dbContext;

    public StockRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Stock>> GetByProductIdAsync(int productId, bool sortByExpirationDate = false)
    {
        var query = _dbContext.Stocks
            .Where(s => s.ProductId == productId);

        if (sortByExpirationDate)
        {
            query = query.OrderBy(s => s.ExpirationDate);
        }

        var answ = await query.ToListAsync();
        return answ;
    }

    public async Task<List<int>> TryConsumeAvailableAmountsAsync(Dictionary<int, decimal> requiredAmounts)
    {
        if (requiredAmounts.Count == 0) return new List<int>();

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        var productIds = requiredAmounts.Keys.ToList();
        var stocks = await _dbContext.Stocks
            .Where(s => productIds.Contains(s.ProductId)
                        && s.Status == Stock.StockStatus.Active
                        && s.ExpirationDate > DateTime.UtcNow
                        && s.Count > 0)
            .OrderBy(s => s.ExpirationDate)
            .ToListAsync();

        var availableAmounts = stocks
            .GroupBy(s => s.ProductId)
            .ToDictionary(group => group.Key, group => group.Sum(s => s.Count));

        var insufficientProductIds = requiredAmounts
            .Where(item => !availableAmounts.TryGetValue(item.Key, out var amount) || amount < item.Value)
            .Select(item => item.Key)
            .ToList();

        if (insufficientProductIds.Count > 0)
        {
            await transaction.CommitAsync();
            return insufficientProductIds;
        }

        foreach (var requirement in requiredAmounts)
        {
            var remaining = requirement.Value;
            foreach (var stock in stocks.Where(s => s.ProductId == requirement.Key))
            {
                var consumed = Math.Min(stock.Count, remaining);
                stock.Count -= consumed;
                remaining -= consumed;
                if (remaining == 0) break;
            }
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return insufficientProductIds;
    }

    public async Task<List<Stock>> GetLastDayExpiredAsync()
    {
        var answ = await _dbContext.Stocks
            .Where(s =>
            s.Status == Stock.StockStatus.Active && 
            s.ExpirationDate - DateTime.UtcNow < TimeSpan.FromDays(1))
            .Include(s => s.Product)
            .ToListAsync();
        return answ;
    }

    public async Task<List<Stock>> GetExpiringSoonAsync()
    {
        var answ = await _dbContext.Stocks
            .Where(s =>
            s.Status == Stock.StockStatus.Active &&
            s.Count > 0 &&
            s.ExpirationDate > DateTime.UtcNow &&
            s.ExpirationDate - DateTime.UtcNow <= TimeSpan.FromDays(2))
            .Include(s => s.Product)
            .Include(s => s.Delivery)
            .AsNoTracking()
            .ToListAsync();
        return answ;
    }

    public async Task<List<Stock>> GetAvailableAsync()
    {
        var answ = await _dbContext.Stocks
            .Where(s => s.Status == Stock.StockStatus.Active && s.ExpirationDate > DateTime.UtcNow && s.Count > 0)
            .Include(s => s.Product)
            .Include(s => s.Delivery)
            .AsNoTracking()
            .ToListAsync();
        return answ;
    }

    public async Task<List<Stock>> GetExpiredActiveAsync()
    {
        var answ = await _dbContext.Stocks
            .Where(s => s.Status == Stock.StockStatus.Active && s.ExpirationDate <= DateTime.UtcNow)
            .Include(s => s.Product)
            .Include(s => s.Delivery)
            .ToListAsync();
        return answ;
    }

    public async Task<List<Stock>> GetExpiredAsync()
    {
        var answ = await _dbContext.Stocks
            .Where(s => s.Status == Stock.StockStatus.Expired)
            .Include(s => s.Product)
            .Include(s => s.Delivery)
            .ToListAsync();
        return answ;
    }

    public async Task<decimal> GetDecommissionedAmountAsync(DateTime from)
    {
        var answ = await _dbContext.Stocks
            .Where(s => (s.Status == Stock.StockStatus.Decommissioned || s.Status == Stock.StockStatus.Expired)
                        && s.ExpirationDate >= from)
            .SumAsync(s => s.Count * s.ItemPrice);
        return answ;
    }
    
    public async Task<Stock?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Stocks.FindAsync(id);
        return answ;
    }

    public async Task<List<Stock>> GetByDeliveryId(int deliveryId)
    {
        var answ = await _dbContext.Stocks
            .Where(s => s.DeliveryId == deliveryId)
            .Include(s => s.Delivery)
            .Include(s => s.Product)
            .AsNoTracking()
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(Stock stock)
    {
        _dbContext.Stocks.Add(stock);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRangeAsync(List<Stock> stocks)
    {
        _dbContext.Stocks.AddRange(stocks);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Stock stock)
    {
        _dbContext.Stocks.Update(stock);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Stock stock)
    {
        _dbContext.Stocks.Remove(stock);
        await _dbContext.SaveChangesAsync();
    }
}
