using System.Text.Json.Serialization;

namespace Domain.DTO;

public class MonthlyFinanceDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }

    [JsonConstructor]
    public MonthlyFinanceDto() { }
}
