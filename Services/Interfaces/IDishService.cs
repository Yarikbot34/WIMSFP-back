using Domain.DTO;

namespace Services.Interfaces;

public interface IDishService
{
    Task<MenuDto> GetMenuAsync();
    Task<DishDto> GetDishAsync(string dishName);

    Task AddDishAsync(DishBigDto dto);
    Task UpdateDishAsync(DishBigDto dto);
    Task ToggleDishAvailabilityAsync(string name);
}
