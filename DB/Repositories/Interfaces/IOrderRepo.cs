using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IOrderRepo
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByStatusAsync(Order.Status status);

    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);

    Task DeleteAsync(Order order);
}
