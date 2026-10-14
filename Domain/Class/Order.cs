namespace Domain.Class;

public class Order
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public Status OrderStatus { get; set; }
    public List<OrderObj> Objs { get; set; }
    public double PayAmount { get; set; }
    public string CustomerName { get; set; }
    public string ContactPhone { get; set; }
    public OrderFormat Format { get; set; }
    public string Comment { get; set; }

    public enum Status
    {
        Complete,
        Cancelled
    }

    public enum OrderFormat
    {
        SelfPickup,
        Hall
    }
}