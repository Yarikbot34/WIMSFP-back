using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IDishRepo
{
    Task<List<Dish>> GetAllAsync();
    Task<List<Dish>> GetAllWithIngredientsAsync();
    Task<Dish?> GetByIdAsync(int id);
    Task<Dish?> GetByNameAsync(string name);
    Task<List<Dish>> GetByNamesWithIngredientsAsync(List<string> names);
    Task UpdateRangeAsync(List<Dish> dishes);

    Task CreateAsync(Dish dish);
    Task UpdateAsync(Dish dish);

    Task DeleteAsync(Dish dish);
}
