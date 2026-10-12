using Domain.DTO;

namespace Services.Interfaces;

public interface IStockService
{
    Task<List<GroupedStockDto>> GetGroupedStocksAsync();
}