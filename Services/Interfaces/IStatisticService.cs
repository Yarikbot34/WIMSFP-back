using Domain.DTO;

namespace Services.Interfaces;

public interface IStatisticService
{
    Task<List<HotOfferDto>> GetHotOffersAsync();
    Task<List<MonthlyFinanceDto>> GetFinanceAnalyticsAsync();
    Task<DecommissionedStockDto> GetDecommissionedStockAsync();
    Task<List<TopDishDto>> GetTopDishesAsync();
    Task<int> GetExpiredSoonCountAsync();
    Task<int> GetExpiredCountAsync();
}
