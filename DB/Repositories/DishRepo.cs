using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class DishRepo : IDishRepo
{
    private readonly AppDbContext _dbContext;

    public DishRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Dish>> GetAllAsync()
    {
        var answ = await _dbContext.Dishes.ToListAsync();
        return answ;
    }

    public async Task<List<Dish>> GetAllWithIngredientsAsync()
    {
        var answ = await _dbContext.Dishes
            .Include(d => d.Ingredients)
            .ThenInclude(i => i.Product)
            .ToListAsync();
        return answ;
    }

    public async Task<Dish?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Dishes.FindAsync(id);
        return answ;
    }

    public async Task<Dish?> GetByNameAsync(string name)
    {
        var answ = await _dbContext.Dishes
            .FirstOrDefaultAsync(d => d.Name == name);
        return answ;
    }

    public async Task<List<Dish>> GetByNamesWithIngredientsAsync(List<string> names)
    {
        var answ = await _dbContext.Dishes
            .Where(d => names.Contains(d.Name))
            .Include(d => d.Ingredients)
                .ThenInclude(i => i.Product)
            .ToListAsync();
        return answ;
    }

    public async Task UpdateRangeAsync(List<Dish> dishes)
    {
        _dbContext.Dishes.UpdateRange(dishes);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(Dish dish)
    {
        _dbContext.Dishes.Add(dish);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Dish dish)
    {
        _dbContext.Dishes.Update(dish);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Dish dish)
    {
        _dbContext.Dishes.Remove(dish);
        await _dbContext.SaveChangesAsync();
    }
}
