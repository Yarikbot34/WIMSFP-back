using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepo _orderR;
    private readonly IDishRepo _dishR;
    private readonly IStockRepo _stockR;
    private readonly IForecastService _forecastS;

    public OrderService(IOrderRepo orderR, IDishRepo dishR, IStockRepo stockR, IForecastService forecastS)
    {
        _orderR = orderR;
        _dishR = dishR;
        _stockR = stockR;
        _forecastS = forecastS;
    }

    public async Task<OrderDto> MakeOrderAsync(OrderDto dto)
    {
        if (dto.Dishes is null || dto.Dishes.Count == 0)
            throw new Exception("Order has no dishes");
        if (dto.Dishes.Any(d => d.Count <= 0))
            throw new Exception("Dish count must be greater than zero");

        var dishNames = dto.Dishes.Select(d => d.DishName).Distinct().ToList();
        var dishes = await _dishR.GetByNamesWithIngredientsAsync(dishNames);
        if (dishes.Count != dishNames.Count)
            throw new Exception("Dish not found");

        var dishesByName = dishes.ToDictionary(d => d.Name);
        var requiredAmounts = new Dictionary<int, decimal>();

        foreach (var orderDish in dto.Dishes)
        {
            var dish = dishesByName[orderDish.DishName];
            foreach (var ingredient in dish.Ingredients)
            {
                var requiredAmount = (decimal)ingredient.Amount * orderDish.Count;
                if (requiredAmount <= 0) continue;
                requiredAmounts[ingredient.ProductId] = requiredAmounts.GetValueOrDefault(ingredient.ProductId) + requiredAmount;
            }
        }

        var insufficientProductIds = await _stockR.TryConsumeAvailableAmountsAsync(requiredAmounts);
        var orderStatus = insufficientProductIds.Count == 0
            ? Order.Status.Complete
            : Order.Status.Cancelled;

        if (insufficientProductIds.Count > 0)
        {
            var insufficientProductIdSet = insufficientProductIds.ToHashSet();
            var unavailableDishes = dishes
                .Where(d => d.Ingredients.Any(i => insufficientProductIdSet.Contains(i.ProductId)))
                .ToList();

            foreach (var dish in unavailableDishes)
            {
                dish.Available = false;
            }

            if (unavailableDishes.Count > 0)
                await _dishR.UpdateRangeAsync(unavailableDishes);
        }

        var order = new Order
        {
            Created = dto.Created == default ? DateTime.UtcNow : dto.Created,
            OrderStatus = orderStatus,
            CustomerName = dto.CustomerName,
            ContactPhone = dto.ContactPhone,
            Format = dto.Format,
            Comment = dto.Comment,
            Objs = new List<OrderObj>(),
            PayAmount = 0
        };

        foreach (var orderDish in dto.Dishes)
        {
            var dish = dishesByName[orderDish.DishName];
            var itemPrice = dish.Price;
            order.Objs.Add(new OrderObj
            {
                Order = order,
                Dish = dish,
                Count = orderDish.Count,
                ItemPrice = itemPrice,
                HasDiscount = false
            });
            order.PayAmount += (double)(itemPrice * orderDish.Count);
        }

        await _orderR.CreateAsync(order);

        if (order.OrderStatus == Order.Status.Complete)
        {
            var consumedDishIds = dishes.Select(d => d.Id).Distinct().ToList();
            await _forecastS.RecalculateDishCountsAsync(consumedDishIds);
        }

        if (order.OrderStatus == Order.Status.Cancelled) throw new Exception("Order cancelled");
        return new OrderDto(order);
    }

    public async Task<List<OrderDto>> GetOrderListAsync()
    {
        var orders = await _orderR.GetAllWithDishesAsync();
        return orders.Select(order => new OrderDto(order)).ToList();
    }
}
