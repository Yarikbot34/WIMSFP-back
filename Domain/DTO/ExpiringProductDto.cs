using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class ExpiringProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int DaysUntilExpiration { get; set; }

    [JsonConstructor]
    public ExpiringProductDto() { }

    public ExpiringProductDto(Stock stock)
    {
        ProductName = stock.Product.Name;
        Amount = stock.Count;
        ExpirationDate = stock.ExpirationDate;
        DaysUntilExpiration = (int)Math.Floor((stock.ExpirationDate - DateTime.UtcNow).TotalDays);
    }
}
