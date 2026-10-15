using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IDeliveryRepo
{
    Task<List<Delivery>> GetAllAsync();
    Task<Delivery?> GetByIdAsync(int id);
    Task<Delivery?> GetByNumberAsync(string number);
    Task<List<Delivery>> GetByDateAsync(DateOnly date);
    Task<List<Delivery>> GetAllWithDateRangeAsync(DateTime from, DateTime to);
    Task<int> CountReceivedByDateRangeAsync(DateTime fromInclusive, DateTime toInclusive);
    
    Task CreateAsync(Delivery delivery);
    Task UpdateAsync(Delivery delivery);
    
    Task DeleteAsync(Delivery delivery);
}