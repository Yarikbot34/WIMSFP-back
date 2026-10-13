using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _dbContext;

    public OrderRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        var answ = await _dbContext.Orders.ToListAsync();
        return answ;
    }

    public async Task<List<Order>> GetAllWithDishesAsync()
    {
        var answ = await _dbContext.Orders
            .Include(o => o.Objs)
                .ThenInclude(o => o.Dish)
            .AsNoTracking()
            .ToListAsync();
        return answ;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Orders.FindAsync(id);
        return answ;
    }

    public async Task<List<Order>> GetByStatusAsync(Order.Status status)
    {
        var answ = await _dbContext.Orders
            .Where(o => o.OrderStatus == status)
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
    }
}
