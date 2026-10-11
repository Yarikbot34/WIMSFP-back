using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class OrderObjRepo : IOrderObjRepo
{
    private readonly AppDbContext _dbContext;

    public OrderObjRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderObj>> GetAllAsync()
    {
        var answ = await _dbContext.OrderObjs.ToListAsync();
        return answ;
    }

    public async Task<OrderObj?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.OrderObjs.FindAsync(id);
        return answ;
    }

    public async Task<List<OrderObj>> GetByOrderIdAsync(int orderId)
    {
        var answ = await _dbContext.OrderObjs
            .Where(o => o.OrderId == orderId)
            .ToListAsync();
        return answ;
    }

    public async Task<List<OrderObj>> GetByDishIdAsync(int dishId)
    {
        var answ = await _dbContext.OrderObjs
            .Where(o => o.DishId == dishId)
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(OrderObj orderObj)
    {
        _dbContext.OrderObjs.Add(orderObj);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderObj orderObj)
    {
        _dbContext.OrderObjs.Update(orderObj);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrderObj orderObj)
    {
        _dbContext.OrderObjs.Remove(orderObj);
        await _dbContext.SaveChangesAsync();
    }
}
