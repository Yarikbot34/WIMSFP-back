using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IDishIngredientRepo
{
    Task<List<DishIngredient>> GetAllAsync();
    Task<DishIngredient?> GetByIdAsync(int id);
    Task<List<DishIngredient>> GetByDishIdAsync(int dishId);
    Task<List<DishIngredient>> GetByProductIdAsync(int productId);

    Task CreateAsync(DishIngredient dishIngredient);
    Task UpdateAsync(DishIngredient dishIngredient);

    Task DeleteAsync(DishIngredient dishIngredient);
}
