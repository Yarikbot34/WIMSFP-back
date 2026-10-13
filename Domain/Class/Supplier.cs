namespace Domain.Class;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public List<Delivery> Deliveries { get; set; }
}