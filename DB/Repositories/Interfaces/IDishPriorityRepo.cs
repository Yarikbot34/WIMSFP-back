using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IDishPriorityRepo
{
    Task<List<DishPriority>> GetAllAsync();
    Task<DishPriority?> GetByDishIdAsync(int dishId);

    Task CreateAsync(DishPriority dishPriority);
    Task UpdateAsync(DishPriority dishPriority);
    Task CreateRangeAsync(List<DishPriority> dishPriorities);
    Task UpdateRangeAsync(List<DishPriority> dishPriorities);

    Task DeleteAsync(DishPriority dishPriority);
}
