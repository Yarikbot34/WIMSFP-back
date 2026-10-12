using DB.Repositories.Interfaces;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class StockService : IStockService
{
    private readonly IStockRepo _stockR;
    
    public StockService(IStockRepo stockR)
    {
        _stockR = stockR;
    }

    public async Task<List<GroupedStockDto>> GetGroupedStocksAsync()
    {
        var stocks = await _stockR.GetAvailableAsync();
        
        HashSet<string> products = stocks
            .Select(x => x.Product.Name)
            .ToHashSet();

        List<GroupedStockDto> answ = new();
        foreach (var type in products)
        {
            var selection = stocks.Where(x => x.Product.Name == type);
            if (selection.Count() == 0) continue;
            
            decimal summ = selection
                .Sum(x => x.Count);

            DateTime fExpired = selection.Min(s => s.ExpirationDate);
            DateTime lDelivery = selection.Max(s => s.Delivery.DateReceived);
            GroupedStockDto n = new GroupedStockDto();
            n.Name = type;
            n.FirstExpired = fExpired;
            n.LastDelivery = lDelivery;
            n.SumAmount = summ;
            n.Stocks = selection.Select(s => new StockDto(s)).ToList();
            answ.Add(n);
        }
        return answ;
    }
}