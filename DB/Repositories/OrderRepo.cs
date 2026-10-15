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

    public async Task<List<Order>> GetCompletedWithDateRangeAsync(DateTime from, DateTime to)
    {
        var answ = await _dbContext.Orders
            .Where(o => o.OrderStatus == Order.Status.Complete
                        && o.Created >= from && o.Created < to)
            .AsNoTracking()
            .ToListAsync();
        return answ;
    }

    public async Task<List<(string DishName, int SoldCount)>> GetTopDishesAsync(DateTime from, DateTime to, int count)
    {
        var answ = await _dbContext.OrderObjs
            .Where(o => o.Order.OrderStatus == Order.Status.Complete
                        && o.Order.Created >= from && o.Order.Created < to)
            .GroupBy(o => o.Dish.Name)
            .Select(group => new
            {
                DishName = group.Key,
                SoldCount = group.Sum(o => o.Count)
            })
            .OrderByDescending(item => item.SoldCount)
            .Take(count)
            .AsNoTracking()
            .ToListAsync();

        return answ
            .Select(item => (item.DishName, item.SoldCount))
            .ToList();
    }

    public async Task CreateAsync(Order order)
    {
        var trackedDishes = new Dictionary<int, Dish>();

        foreach (var orderObj in order.Objs)
        {
            var dishId = orderObj.DishId != 0 ? orderObj.DishId : orderObj.Dish.Id;

            if (!trackedDishes.TryGetValue(dishId, out var trackedDish))
            {
                trackedDish = _dbContext.Dishes.Local.FirstOrDefault(d => d.Id == dishId);

                if (trackedDish is null)
                {
                    var sourceDish = orderObj.Dish;
                    trackedDish = new Dish
                    {
                        Id = dishId,
                        Name = sourceDish.Name,
                        ImageUrl = sourceDish.ImageUrl,
                        Category = sourceDish.Category,
                        Ingredients = new List<DishIngredient>()
                    };
                    _dbContext.Dishes.Attach(trackedDish);
                }

                trackedDishes.Add(dishId, trackedDish);
            }

            orderObj.DishId = dishId;
            orderObj.Dish = trackedDish;
        }

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
