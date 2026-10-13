namespace Domain.DTO;

public class MenuDto
{
    public List<DishDto> Popular { get; set; } = new();
    public List<DishDto> Main { get; set; } = new();
    public List<DishDto> Inaccessible { get; set; } = new();
}