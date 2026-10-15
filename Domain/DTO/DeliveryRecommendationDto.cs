using System.Text.Json.Serialization;

namespace Domain.DTO;

public class DeliveryRecommendationDto
{
    public string ProductName { get; set; } = string.Empty;
    public string Unit { get; set; } = "Item";
    public decimal RequiredAmount { get; set; }
    public decimal AvailableAmount { get; set; }
    public decimal ToBuyAmount { get; set; }

    [JsonConstructor]
    public DeliveryRecommendationDto() { }
}
