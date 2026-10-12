namespace Domain.DTO;

public class StockDto
{
    public string ProductType { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateExpired { get; set; }
    public DateTime DateReceived { get; set; }
}