namespace Domain.Class;

public class DishIngredient
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int Amount { get; set; }
}