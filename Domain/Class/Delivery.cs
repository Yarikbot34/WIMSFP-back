namespace Domain.Class;

public class Delivery
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public DateTime DateReceived { get; set; }
}