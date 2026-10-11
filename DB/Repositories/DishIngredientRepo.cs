using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class DishIngredientRepo : IDishIngredientRepo
{
    private readonly AppDbContext _dbContext;

    public DishIngredientRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DishIngredient>> GetAllAsync()
    {
        var answ = await _dbContext.DishIngredients.ToListAsync();
        return answ;
    }

    public async Task<DishIngredient?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.DishIngredients.FindAsync(id);
        return answ;
    }

    public async Task<List<DishIngredient>> GetByDishIdAsync(int dishId)
    {
        var answ = await _dbContext.DishIngredients
            .Where(di => di.DishId == dishId)
            .ToListAsync();
        return answ;
    }

    public async Task<List<DishIngredient>> GetByProductIdAsync(int productId)
    {
        var answ = await _dbContext.DishIngredients
            .Where(di => di.ProductId == productId)
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(DishIngredient dishIngredient)
    {
        _dbContext.DishIngredients.Add(dishIngredient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DishIngredient dishIngredient)
    {
        _dbContext.DishIngredients.Update(dishIngredient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DishIngredient dishIngredient)
    {
        _dbContext.DishIngredients.Remove(dishIngredient);
        await _dbContext.SaveChangesAsync();
    }
}
