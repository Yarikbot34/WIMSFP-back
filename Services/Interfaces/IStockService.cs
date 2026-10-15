using Domain.DTO;

namespace Services.Interfaces;

public interface IStockService
{
    Task<List<GroupedStockDto>> GetGroupedStocksAsync();
    Task<List<StockDto>> GetExpiredSoonAsync();
    Task<List<StockDto>> GetExpiredAsync();
    Task WriteOffStockAsync(int id);
}