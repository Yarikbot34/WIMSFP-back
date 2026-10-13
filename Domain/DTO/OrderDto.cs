namespace Domain.DTO;

public class OrderDto
{
    public DateTime Created { get; set; }
    public decimal Payment { get; set; }
    public List<OrderObjDto> Dishes { get; set; }
}