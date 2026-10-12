using Domain.Class;

namespace Domain.DTO;

public class StockDto
{
    public string ProductType { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateExpired { get; set; }
    public DateTime DateReceived { get; set; }
    
    public StockDto(){}

    public StockDto(Stock stock)
    {
        ProductType = stock.Product.Name;
        Amount = stock.Count;
        DateExpired = stock.ExpirationDate;
        DateReceived = stock.Delivery.DateReceived;
    }
}