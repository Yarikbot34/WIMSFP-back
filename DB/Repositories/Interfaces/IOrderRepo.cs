using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IOrderRepo
{
    Task<List<Order>> GetAllAsync();
    Task<List<Order>> GetAllWithDishesAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByStatusAsync(Order.Status status);
    Task<List<Order>> GetCompletedWithDateRangeAsync(DateTime from, DateTime to);
    Task<List<(string DishName, int SoldCount)>> GetTopDishesAsync(DateTime from, DateTime to, int count);

    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);

    Task DeleteAsync(Order order);
}
