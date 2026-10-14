using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class OrderDto
{
    public DateTime Created { get; set; }
    public decimal Payment { get; set; }
    public Order.Status Status { get; set; }
    public string CustomerName { get; set; }
    public string ContactPhone { get; set; }
    public Order.OrderFormat Format { get; set; }
    public string Comment { get; set; }
    public List<OrderObjDto> Dishes { get; set; } = new();

    [JsonConstructor]
    public OrderDto() { }

    public OrderDto(Order order)
    {
        Created = order.Created;
        Payment = (decimal)order.PayAmount;
        Status = order.OrderStatus;
        CustomerName = order.CustomerName;
        ContactPhone = order.ContactPhone;
        Format = order.Format;
        Comment = order.Comment;
        Dishes = order.Objs.Select(orderObj => new OrderObjDto(orderObj)).ToList();
    }
}