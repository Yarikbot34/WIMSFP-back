using Domain.DTO;

namespace Services.Interfaces;

public interface IDishService
{
    Task<MenuDto> GetMenuAsync();
    Task<DishDto> GetDishAsync(string dishName);
}