using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IStockRepo
{
    public Task<List<Stock>> GetByProductIdAsync(int productId, bool sortByExpirationDate = false);
    public Task<List<Stock>> GetAvailableAsync();
    public Task<List<Stock>> GetLastDayExpiredAsync();
    public Task<List<Stock>> GetExpiringSoonAsync();
    public Task<List<int>> TryConsumeAvailableAmountsAsync(Dictionary<int, decimal> requiredAmounts);
    public Task<List<Stock>> GetExpiredActiveAsync();
    public Task<List<Stock>> GetExpiredAsync();
    public Task<decimal> GetDecommissionedAmountAsync(DateTime from);
    public Task<Stock?> GetByIdAsync(int id);
    public Task<List<Stock>> GetByDeliveryId(int deliveryId);
    
    public Task CreateAsync(Stock stock);
    public Task CreateRangeAsync(List<Stock> stocks);
    public Task UpdateAsync(Stock stock);
    
    public Task DeleteAsync(Stock stock);
}