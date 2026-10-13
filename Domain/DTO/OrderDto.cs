using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class OrderDto
{
    public DateTime Created { get; set; }
    public decimal Payment { get; set; }
    public Order.Status Status { get; set; }
    public List<OrderObjDto> Dishes { get; set; } = new();

    [JsonConstructor]
    public OrderDto() { }

    public OrderDto(Order order)
    {
        Created = order.Created;
        Payment = (decimal)order.PayAmount;
        Status = order.OrderStatus;
        Dishes = order.Objs.Select(orderObj => new OrderObjDto(orderObj)).ToList();
    }
}