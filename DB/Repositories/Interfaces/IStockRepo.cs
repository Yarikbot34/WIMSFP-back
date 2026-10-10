using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IStockRepo
{
    public Task<List<Stock>> GetByProductIdAsync(int productId, bool sortByExpirationDate = false);
    public Task<Stock?> GetByIdAsync(int id);
    public Task<List<Stock>> GetByDeliveryId(int deliveryId);
    
    public Task CreateAsync(Stock stock);
    public Task UpdateAsync(Stock stock);
    
    public Task DeleteAsync(Stock stock);
}