using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class OrderObjDto
{
    public string DishName { get; set; } = string.Empty;
    public decimal DishPrice { get; set; }
    public int Count { get; set; }

    [JsonConstructor]
    public OrderObjDto() { }

    public OrderObjDto(OrderObj orderObj)
    {
        DishName = orderObj.Dish.Name;
        DishPrice = orderObj.ItemPrice;
        Count = orderObj.Count;
    }
}