using DB.Repositories.Interfaces;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class StatisticService : IStatisticService
{
    private readonly IDishRepo _dishR;
    private readonly IStockRepo _stockR;
    private readonly IDishPriorityRepo _dishPriorityR;
    private readonly IDeliveryRepo _deliveryR;
    private readonly IOrderRepo _orderR;

    public StatisticService(
        IDishRepo dishR,
        IStockRepo stockR,
        IDishPriorityRepo dishPriorityR,
        IDeliveryRepo deliveryR,
        IOrderRepo orderR)
    {
        _dishR = dishR;
        _stockR = stockR;
        _dishPriorityR = dishPriorityR;
        _deliveryR = deliveryR;
        _orderR = orderR;
    }

    public async Task<List<HotOfferDto>> GetHotOffersAsync()
    {
        var expiringStocks = await _stockR.GetExpiringSoonAsync();
        if (expiringStocks.Count == 0) return new List<HotOfferDto>();

        var expiringProductIds = expiringStocks
            .Select(s => s.ProductId)
            .Distinct()
            .ToHashSet();

        var priorities = await _dishPriorityR.GetAllAsync();
        var hotDishIds = priorities
            .Where(p => p.Priority > 0)
            .OrderByDescending(p => p.Priority)
            .Select(p => p.DishId)
            .ToList();

        if (hotDishIds.Count == 0) return new List<HotOfferDto>();

        var dishes = await _dishR.GetByIdsWithIngredientsAsync(hotDishIds);
        var priorityByDishId = priorities.ToDictionary(p => p.DishId, p => p.Priority);
        var expiringByProductId = expiringStocks
            .GroupBy(s => s.ProductId)
            .ToDictionary(g => g.Key, g => g.ToList());

        List<HotOfferDto> answ = new();
        foreach (var dish in dishes)
        {
            var expiringProducts = dish.Ingredients
                .Where(i => expiringProductIds.Contains(i.ProductId))
                .SelectMany(i => expiringByProductId[i.ProductId])
                .Select(s => new ExpiringProductDto(s))
                .ToList();

            if (expiringProducts.Count == 0) continue;

            answ.Add(new HotOfferDto
            {
                DishName = dish.Name,
                Priority = priorityByDishId.GetValueOrDefault(dish.Id),
                ExpiringProducts = expiringProducts
            });
        }

        return answ
            .OrderByDescending(o => o.Priority)
            .ToList();
    }

    public async Task<List<MonthlyFinanceDto>> GetFinanceAnalyticsAsync()
    {
        var now = DateTime.UtcNow;
        var from = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-11);

        var deliveries = await _deliveryR.GetAllWithDateRangeAsync(from, now.AddMonths(1));
        var orders = await _orderR.GetCompletedWithDateRangeAsync(from, now.AddMonths(1));

        var expenseByMonth = deliveries
            .GroupBy(d => (d.DateReceived.Year, d.DateReceived.Month))
            .ToDictionary(g => g.Key, g => g.Sum(d => d.Price));

        var incomeByMonth = orders
            .GroupBy(o => (o.Created.Year, o.Created.Month))
            .ToDictionary(g => g.Key, g => g.Sum(o => (decimal)o.PayAmount));

        List<MonthlyFinanceDto> answ = new();
        for (int i = 0; i < 12; i++)
        {
            var month = from.AddMonths(i);

            answ.Add(new MonthlyFinanceDto
            {
                Year = month.Year,
                Month = month.Month,
                Income = incomeByMonth.GetValueOrDefault((month.Year, month.Month)),
                Expense = expenseByMonth.GetValueOrDefault((month.Year, month.Month))
            });
        }

        return answ;
    }

    public async Task<DecommissionedStockDto> GetDecommissionedStockAsync()
    {
        var from = DateTime.UtcNow.AddMonths(-1);
        var amount = await _stockR.GetDecommissionedAmountAsync(from);

        return new DecommissionedStockDto
        {
            TotalAmount = amount
        };
    }

    public async Task<List<TopDishDto>> GetTopDishesAsync()
    {
        var now = DateTime.UtcNow;
        var from = now.AddMonths(-1);

        var topDishes = await _orderR.GetTopDishesAsync(from, now.AddMonths(1), 3);

        return topDishes
            .Select(item => new TopDishDto
            {
                DishName = item.DishName,
                SoldCount = item.SoldCount
            })
            .ToList();
    }

    public async Task<int> GetExpiredSoonCountAsync()
    {
        return await _stockR.GetExpiringSoonCountAsync();
    }

    public async Task<int> GetExpiredCountAsync()
    {
        return await _stockR.GetExpiredNotDecommissionedCountAsync();
    }
}
