namespace Domain.DTO;

public class MenuDto
{
    public List<DishDto> Popular { get; set; }
    public List<DishDto> Main { get; set; }
    public List<DishDto> Inaccessible { get; set; }
}