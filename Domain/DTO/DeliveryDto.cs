using Domain.Class;
namespace Domain.DTO;

public class DeliveryDto
{
    public required string Number { get; set; }
    public DateTime DateReceived { get; set; }
    public required List<Stock> ReceivedProducts { get; set; }
}