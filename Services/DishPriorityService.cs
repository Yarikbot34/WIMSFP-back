using DB.Repositories.Interfaces;
using Domain.Class;
using Services.Interfaces;

namespace Services;

public class DishPriorityService : IDishPriorityService
{
    private readonly IDishRepo _dishR;
    private readonly IStockRepo _stockR;
    private readonly IDishPriorityRepo _dishPriorityR;

    public DishPriorityService(IDishRepo dishR, IStockRepo stockR, IDishPriorityRepo dishPriorityR)
    {
        _dishR = dishR;
        _stockR = stockR;
        _dishPriorityR = dishPriorityR;
    }

    public async Task RecalculatePrioritiesAsync()
    {
        var expiringStocks = await _stockR.GetExpiringSoonAsync();
        var expiringProductIds = expiringStocks
            .Select(s => s.ProductId)
            .Distinct()
            .ToHashSet();

        var dishes = await _dishR.GetAllWithIngredientsAsync();
        var existingPriorities = await _dishPriorityR.GetAllAsync();
        var prioritiesByDishId = existingPriorities
            .GroupBy(p => p.DishId)
            .ToDictionary(g => g.Key, g => g.First());

        var duplicatePriorities = existingPriorities
            .GroupBy(p => p.DishId)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g.OrderByDescending(p => p.Id).Skip(1))
            .ToList();

        var existingDishIds = dishes.Select(d => d.Id).ToHashSet();
        var stalePriorities = existingPriorities
            .Where(p => !existingDishIds.Contains(p.DishId))
            .ToList();

        var toCreate = new List<DishPriority>();
        var toUpdate = new List<DishPriority>();

        foreach (var dish in dishes)
        {
            var priority = CalculatePriority(dish, expiringProductIds);

            if (prioritiesByDishId.TryGetValue(dish.Id, out var existing))
            {
                existing.Priority = priority;
                toUpdate.Add(existing);
            }
            else
            {
                toCreate.Add(new DishPriority
                {
                    DishId = dish.Id,
                    Priority = priority
                });
            }
        }

        if (toCreate.Count > 0) await _dishPriorityR.CreateRangeAsync(toCreate);
        if (toUpdate.Count > 0) await _dishPriorityR.UpdateRangeAsync(toUpdate);

        var toDelete = duplicatePriorities
            .Concat(stalePriorities)
            .DistinctBy(p => p.Id)
            .ToList();
        if (toDelete.Count > 0) await _dishPriorityR.DeleteRangeAsync(toDelete);
    }

    private static decimal CalculatePriority(Dish dish, HashSet<int> expiringProductIds)
    {
        if (dish.Ingredients.Count == 0) return 0m;

        var expiringCount = dish.Ingredients
            .Count(i => expiringProductIds.Contains(i.ProductId));

        return (decimal)expiringCount / dish.Ingredients.Count;
    }
}
