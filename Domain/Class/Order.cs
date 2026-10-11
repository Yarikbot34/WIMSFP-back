namespace Domain.Class;

public class Order
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public Status OrderStatus { get; set; }
    public List<OrderObj> Objs { get; set; }
        
        
    public enum Status
    {
        New,
        Processing,
        Complete,
        Cancelled
    }
}