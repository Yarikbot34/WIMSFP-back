namespace Domain.DTO;

public class ResProductDto
{
    public required string ProductName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Count { get; set; }
}