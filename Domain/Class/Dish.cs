namespace Domain.Class;

public class Dish
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public required string Category { get; set; } = "Разное";
    public string Description { get; set; } = "";
    public required List<DishIngredient> Ingredients { get; set; }

    public bool Available { get; set; } = false;
    public bool HasAction { get; set; } = false;
    public decimal Price { get; set; } = 0;
    public int CountCanPrepare { get; set; } = 0;
}