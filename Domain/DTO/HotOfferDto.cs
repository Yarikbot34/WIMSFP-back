using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class HotOfferDto
{
    public string DishName { get; set; } = string.Empty;
    public decimal Priority { get; set; }
    public List<ExpiringProductDto> ExpiringProducts { get; set; } = new();

    [JsonConstructor]
    public HotOfferDto() { }
}
