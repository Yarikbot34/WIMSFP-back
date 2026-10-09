namespace Domain.Class;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Unit MesUnit { get; set; }


    public enum Unit
    {
        Kilogram,
        Liter,
        Item
    }
}