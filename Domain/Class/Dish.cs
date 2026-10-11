namespace Domain.Class;

public class Dish
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    public bool Available { get; set; } = false;
}