using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class ExpiredStockDto
{
    public int Id { get; set; }
    public string ProductType { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateExpired { get; set; }
    public DateTime DateReceived { get; set; }
    public string Status { get; set; }
    public int DaysUntilExpiration { get; set; }

    [JsonConstructor]
    public ExpiredStockDto() {}

    public ExpiredStockDto(Stock stock)
    {
        Id = stock.Id;
        ProductType = stock.Product.Name;
        Amount = stock.Count;
        DateExpired = stock.ExpirationDate;
        DateReceived = stock.Delivery.DateReceived;
        Status = stock.Status.ToString();
        DaysUntilExpiration = (int)Math.Floor((stock.ExpirationDate - DateTime.UtcNow).TotalDays);
    }
}
