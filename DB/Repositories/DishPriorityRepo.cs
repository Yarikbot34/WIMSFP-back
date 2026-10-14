using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class DishPriorityRepo : IDishPriorityRepo
{
    private readonly AppDbContext _dbContext;

    public DishPriorityRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DishPriority>> GetAllAsync()
    {
        var answ = await _dbContext.DishPriorities.ToListAsync();
        return answ;
    }

    public async Task<DishPriority?> GetByDishIdAsync(int dishId)
    {
        var answ = await _dbContext.DishPriorities
            .FirstOrDefaultAsync(p => p.DishId == dishId);
        return answ;
    }

    public async Task CreateAsync(DishPriority dishPriority)
    {
        _dbContext.DishPriorities.Add(dishPriority);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DishPriority dishPriority)
    {
        _dbContext.DishPriorities.Update(dishPriority);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRangeAsync(List<DishPriority> dishPriorities)
    {
        _dbContext.DishPriorities.AddRange(dishPriorities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<DishPriority> dishPriorities)
    {
        _dbContext.DishPriorities.UpdateRange(dishPriorities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DishPriority dishPriority)
    {
        _dbContext.DishPriorities.Remove(dishPriority);
        await _dbContext.SaveChangesAsync();
    }
}
