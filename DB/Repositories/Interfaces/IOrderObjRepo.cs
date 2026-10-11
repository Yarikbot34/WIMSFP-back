using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IOrderObjRepo
{
    Task<List<OrderObj>> GetAllAsync();
    Task<OrderObj?> GetByIdAsync(int id);
    Task<List<OrderObj>> GetByOrderIdAsync(int orderId);
    Task<List<OrderObj>> GetByDishIdAsync(int dishId);

    Task CreateAsync(OrderObj orderObj);
    Task UpdateAsync(OrderObj orderObj);

    Task DeleteAsync(OrderObj orderObj);
}
