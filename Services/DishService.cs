using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class DishService : IDishService
{
    private readonly IDishRepo _dishR;
    private readonly IDishIngredientRepo _dishIngredientR;
    private readonly IProductsRepo _productsR;
    private readonly IStockRepo _stockR;

    public DishService(IDishRepo dishR, IDishIngredientRepo dishIngredientR, IProductsRepo productsR, IStockRepo stockR)
    {
        _stockR = stockR;
        _dishR = dishR;
        _dishIngredientR = dishIngredientR;
        _productsR = productsR;
    }

    public async Task<MenuDto> GetMenuAsync()
    {
        await CheckExpiredDishesAsync();
        var dishes =  await _dishR.GetAllAsync();
        var menuDto = new MenuDto();
        foreach (var dish in dishes)
        {
            if (!dish.Available) menuDto.Inaccessible.Add(new DishDto(dish)); 
            else if (dish.HasAction) menuDto.Popular.Add(new DishDto(dish));
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

    public async Task AddDishAsync(DishBigDto dto)
    {
        if (dto.Ingredients.Count == 0) throw new Exception("No ingredients provided");

        var existing = await _dishR.GetByNameAsync(dto.Name);
        if (existing is not null) throw new Exception("Dish already exists");

        var productNames = dto.Ingredients
            .Select(i => i.Product)
            .ToList();

        var products = await _productsR.GetByNameListAsync(productNames);
        if (products.Count != productNames.Distinct().Count()) throw new Exception("Product not found");

        Dish dish = new Dish
        {
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            Description = dto.Description,
            Available = dto.Available,
            Price = dto.Price,
            Ingredients = new List<DishIngredient>()
        };

        foreach (var ingredient in dto.Ingredients)
        {
            var pr = products.Find(p => p.Name == ingredient.Product);
            if (pr is null) throw new Exception("Product not found");
            dish.Ingredients.Add(new DishIngredient
            {
                Dish = dish,
                Product = pr,
                Amount = (int)ingredient.Count
            });
        }

        await _dishR.CreateAsync(dish);
    }

    public async Task UpdateDishAsync(DishBigDto dto)
    {
        var dish = await _dishR.GetByNameAsync(dto.Name);
        if (dish is null) throw new Exception("Dish not found");

        if (dto.Ingredients.Count == 0) throw new Exception("No ingredients provided");

        var productNames = dto.Ingredients
            .Select(i => i.Product)
            .ToList();

        var products = await _productsR.GetByNameListAsync(productNames);
        if (products.Count != productNames.Distinct().Count()) throw new Exception("Product not found");

        dish.Description = dto.Description;
        dish.Price = dto.Price;
        dish.Available = dto.Available;
        dish.ImageUrl = dto.ImageUrl;
        dish.Category = dto.Category;

        var oldIngredients = await _dishIngredientR.GetByDishIdAsync(dish.Id);
        foreach (var ingredient in oldIngredients)
        {
            await _dishIngredientR.DeleteAsync(ingredient);
        }

        dish.Ingredients = new List<DishIngredient>();
        foreach (var ingredient in dto.Ingredients)
        {
            var pr = products.Find(p => p.Name == ingredient.Product);
            if (pr is null) throw new Exception("Product not found");
            dish.Ingredients.Add(new DishIngredient
            {
                Dish = dish,
                Product = pr,
                Amount = (int)ingredient.Count
            });
        }

        await _dishR.UpdateAsync(dish);
    }

    public async Task ToggleDishAvailabilityAsync(string name)
    {
        var dish = await _dishR.GetByNameAsync(name);
        if (dish is null) throw new Exception("Dish not found");

        dish.Available = !dish.Available;
        await _dishR.UpdateAsync(dish);
    }
    
    private async Task CheckExpiredDishesAsync()
    {
        var criticalStoks = await _stockR.GetLastDayExpiredAsync();
        if (criticalStoks.Count == 0) return;
        var productsIds = criticalStoks.Select(x => x.ProductId).Distinct().ToList();

        var dishes = await _dishR.GetAllWithIngredientsAsync();
        foreach (var dish in dishes)
        {
            dish.HasAction = dish.Ingredients.Any(i => productsIds.Contains(i.ProductId));
        }

        await _dishR.UpdateRangeAsync(dishes);

    }
}
