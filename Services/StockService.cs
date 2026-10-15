using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class StockService : IStockService
{
    private readonly IStockRepo _stockR;
    private readonly IProductsRepo _productsR;
    
    public StockService(IStockRepo stockR, IProductsRepo productsR)
    {
        _stockR = stockR;
        _productsR = productsR;
    }

    public async Task<List<GroupedStockDto>> GetGroupedStocksAsync()
    {
        var stocks = await _stockR.GetAvailableAsync();
        var dbProducts = await _productsR.GetAllAsync();
        
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
            n.Unit = dbProducts.First(p => p.Name == type).MesUnit.ToString();
            answ.Add(n);
        }
        return answ;
    }

    public async Task<List<StockDto>> GetExpiredSoonAsync()
    {
        var stocks = await _stockR.GetExpiringSoonOrExpiredNotDecommissionedAsync();
        return stocks.Select(s => new StockDto(s)).ToList();
    }

    public async Task<List<StockDto>> GetExpiredAsync()
    {
        var stocks = await _stockR.GetExpiredAsync();
        return stocks.Select(s => new StockDto(s)).ToList();
    }

    public async Task<List<ExpiredStockDto>> GetExpiredStocksAsync()
    {
        var stocks = await _stockR.GetExpiredAsync();
        return stocks.Select(s => new ExpiredStockDto(s)).ToList();
    }

    public async Task WriteOffStockAsync(int id)
    {
        var stock = await _stockR.GetByIdAsync(id);
        if (stock is null) throw new Exception("Stock not found");

        stock.Status = Stock.StockStatus.Decommissioned;
        await _stockR.UpdateAsync(stock);
    }
}