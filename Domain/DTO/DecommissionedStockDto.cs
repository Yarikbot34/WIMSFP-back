using System.Text.Json.Serialization;

namespace Domain.DTO;

public class DecommissionedStockDto
{
    public decimal TotalAmount { get; set; }

    [JsonConstructor]
    public DecommissionedStockDto() { }
}
