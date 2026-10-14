using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class DishDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
    public bool Available { get; set; }
    
    [JsonConstructor]
    public DishDto(){}


    public DishDto(Dish dish)
    {
        Name = dish.Name;
        Description = dish.Description;
        Price = dish.Price;
        ImageUrl = dish.ImageUrl;
        Category = dish.Category;
        Available = dish.Available;
    }
}