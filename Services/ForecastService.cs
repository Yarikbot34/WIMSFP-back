using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class ForecastService : IForecastService
{
    private const int StopListThreshold = 5;

    private readonly IDishRepo _dishR;
    private readonly IStockRepo _stockR;
    private readonly IProductsRepo _productsR;

    public ForecastService(IDishRepo dishR, IStockRepo stockR, IProductsRepo productsR)
    {
        _dishR = dishR;
        _stockR = stockR;
        _productsR = productsR;
    }

    public async Task RecalculateDishCountsAsync(List<int> dishIds)
    {
        if (dishIds is null || dishIds.Count == 0) return;

        var dishes = await _dishR.GetByIdsWithIngredientsAsync(dishIds);
        if (dishes.Count == 0) return;

        var productIds = dishes
            .SelectMany(d => d.Ingredients)
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var availableAmounts = await _stockR.GetAvailableAmountsByProductIdsAsync(productIds);

        foreach (var dish in dishes)
        {
            dish.CountCanPrepare = CalculateCountCanPrepare(dish, availableAmounts);
        }

        await _dishR.UpdateRangeAsync(dishes);
    }

    public async Task<List<StopListDishDto>> GetStopListAsync()
    {
        var dishes = await _dishR.GetAllWithIngredientsAsync();
        var productIds = dishes
            .SelectMany(d => d.Ingredients)
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var availableAmounts = await _stockR.GetAvailableAmountsByProductIdsAsync(productIds);

        return dishes
            .Select(d => new
            {
                Dish = d,
                Count = CalculateCountCanPrepare(d, availableAmounts)
            })
            .Where(item => item.Count < StopListThreshold)
            .OrderBy(item => item.Count)
            .Select(item => new StopListDishDto
            {
                DishName = item.Dish.Name,
                CountCanPrepare = item.Count
            })
            .ToList();
    }

    public async Task<List<DeliveryRecommendationDto>> GetDeliveryRecommendationsAsync()
    {
        var dishes = await _dishR.GetAllWithIngredientsAsync();
        var productIds = dishes
            .SelectMany(d => d.Ingredients)
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var availableAmounts = await _stockR.GetAvailableAmountsByProductIdsAsync(productIds);

        var forecastDishes = dishes
            .Select(d => new
            {
                Dish = d,
                Count = CalculateCountCanPrepare(d, availableAmounts)
            })
            .Where(item => item.Count < StopListThreshold)
            .ToList();

        var requiredAmounts = new Dictionary<int, decimal>();
        foreach (var item in forecastDishes)
        {
            var missingPortions = StopListThreshold - item.Count;
            if (missingPortions <= 0) continue;

            foreach (var ingredient in item.Dish.Ingredients)
            {
                var requiredAmount = (decimal)ingredient.Amount * missingPortions;
                if (requiredAmount <= 0) continue;
                requiredAmounts[ingredient.ProductId] = requiredAmounts.GetValueOrDefault(ingredient.ProductId) + requiredAmount;
            }
        }

        if (requiredAmounts.Count == 0) return new List<DeliveryRecommendationDto>();

        var products = await _productsR.GetAllAsync();
        var productsById = products.ToDictionary(p => p.Id);

        List<DeliveryRecommendationDto> answ = new();
        foreach (var requirement in requiredAmounts)
        {
            if (!productsById.TryGetValue(requirement.Key, out var product)) continue;

            var available = availableAmounts.GetValueOrDefault(requirement.Key);
            var toBuy = requirement.Value - available;
            if (toBuy <= 0) continue;

            answ.Add(new DeliveryRecommendationDto
            {
                ProductName = product.Name,
                Unit = product.MesUnit.ToString(),
                RequiredAmount = requirement.Value,
                AvailableAmount = available,
                ToBuyAmount = toBuy
            });
        }

        return answ
            .OrderByDescending(item => item.ToBuyAmount)
            .ToList();
    }

    private static int CalculateCountCanPrepare(Dish dish, Dictionary<int, decimal> availableAmounts)
    {
        if (dish.Ingredients.Count == 0) return 0;

        int? minPortions = null;
        foreach (var ingredient in dish.Ingredients)
        {
            if (ingredient.Amount <= 0) return 0;

            var available = availableAmounts.GetValueOrDefault(ingredient.ProductId);
            var portions = (int)Math.Floor(available / ingredient.Amount);

            minPortions = minPortions is null ? portions : Math.Min(minPortions.Value, portions);
        }

        return minPortions ?? 0;
    }
}
