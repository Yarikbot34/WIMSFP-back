using System.Text.Json.Serialization;

namespace Domain.DTO;

public class TopDishDto
{
    public string DishName { get; set; } = string.Empty;
    public int SoldCount { get; set; }

    [JsonConstructor]
    public TopDishDto() { }
}
