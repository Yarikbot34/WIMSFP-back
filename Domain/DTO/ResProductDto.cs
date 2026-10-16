using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Domain.Class;

namespace Domain.DTO;

public class ResProductDto
{
    public required string ProductName { get; set; }
    public string Unit { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Count { get; set; }
    public decimal ItemPrice { get; set; }
    
    [JsonConstructor]
    public ResProductDto(){}

    [SetsRequiredMembers]
    public ResProductDto(Stock stock)
    {
        ProductName = stock.Product.Name;
        Unit = stock.Product.MesUnit.ToString();
        ExpirationDate = stock.ExpirationDate;
        Count = stock.InitialCount;
        ItemPrice = stock.ItemPrice;
    }
}