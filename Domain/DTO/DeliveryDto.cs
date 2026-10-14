using Domain.Class;
namespace Domain.DTO;

public class DeliveryDto
{
    public required string Number { get; set; }
    public DateTime DateReceived { get; set; }
    public decimal Price { get; set; }
    public string SupplierName { get; set; }
    public required List<ResProductDto> ReceivedProducts { get; set; }
}