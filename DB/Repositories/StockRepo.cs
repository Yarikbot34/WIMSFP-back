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

    public async Task<Stock?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Stocks.FindAsync(id);
        return answ;
    }

    public async Task<List<Stock>> GetByDeliveryId(int deliveryId)
    {
        var answ = await _dbContext.Stocks
            .Where(s => s.DeliveryId == deliveryId)
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(Stock stock)
    {
        _dbContext.Stocks.Add(stock);
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
