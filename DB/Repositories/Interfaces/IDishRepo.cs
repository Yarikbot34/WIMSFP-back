using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IDishRepo
{
    Task<List<Dish>> GetAllAsync();
    Task<Dish?> GetByIdAsync(int id);
    Task<Dish?> GetByNameAsync(string name);

    Task CreateAsync(Dish dish);
    Task UpdateAsync(Dish dish);

    Task DeleteAsync(Dish dish);
}
