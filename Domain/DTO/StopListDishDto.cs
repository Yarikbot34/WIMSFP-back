using System.Text.Json.Serialization;

namespace Domain.DTO;

public class StopListDishDto
{
    public string DishName { get; set; } = string.Empty;
    public int CountCanPrepare { get; set; }

    [JsonConstructor]
    public StopListDishDto() { }

    public StopListDishDto(Class.Dish dish)
    {
        DishName = dish.Name;
        CountCanPrepare = dish.CountCanPrepare;
    }
}
