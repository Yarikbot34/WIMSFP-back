using DB.Repositories.Interfaces;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class DishService : IDishService
{
    private readonly IDishRepo _dishR;

    public DishService(IDishRepo dishR)
    {
        _dishR = dishR;
    }

    public async Task<MenuDto> GetMenuAsync()
    {
        var dishes =  await _dishR.GetAllAsync();
        var menuDto = new MenuDto();
        foreach (var dish in dishes)
        {
            if (!dish.Available) menuDto.Inaccessible.Add(new DishDto(dish)); 
            else menuDto.Main.Add(new DishDto(dish));
        }
        return menuDto;
    }

    public async Task<DishDto> GetDishAsync(string dishName)
    {
        var dish = await _dishR.GetByNameAsync(dishName);
        if  (dish == null) throw new FileNotFoundException("Dish not found", dishName);
        return new DishDto(dish);
    }
}