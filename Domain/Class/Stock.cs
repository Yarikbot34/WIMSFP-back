namespace Domain.Class;

public class Stock
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    
    public int DeliveryId { get; set; }
    public required Delivery Delivery { get; set; }
    
    public decimal Count { get; set; }
    public DateTime ExpirationDate { get; set; }
    
}