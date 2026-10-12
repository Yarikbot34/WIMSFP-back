namespace Domain.DTO;

public class GroupedStockDto
{
    public string Name {get; set;}
    public decimal SumAmount {get; set;}
    public DateTime FirstExpired {get; set;}
    public DateTime LastDelivery {get; set;}
    public List<StockDto> Stocks {get; set;}
}