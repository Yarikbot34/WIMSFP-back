using System.Text.Json.Serialization;

namespace Domain.DTO;

public class DishBigDto
{
    public string Name { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public List<DishIngredientDto> Ingredients { get; set; } = new();
    
    [JsonConstructor]
    public DishBigDto(){}
}