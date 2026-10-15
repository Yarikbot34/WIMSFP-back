using Domain.DTO;

namespace Services.Interfaces;

public interface IForecastService
{
    Task RecalculateDishCountsAsync(List<int> dishIds);
    Task<List<StopListDishDto>> GetStopListAsync();
    Task<List<DeliveryRecommendationDto>> GetDeliveryRecommendationsAsync();
}
