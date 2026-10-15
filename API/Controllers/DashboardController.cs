using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IStatisticService _statisticService;
    private readonly IForecastService _forecastService;

    public DashboardController(IStatisticService statisticService, IForecastService forecastService)
    {
        _statisticService = statisticService;
        _forecastService = forecastService;
    }

    [HttpGet("HotOffers")]
    public async Task<IActionResult> GetHotOffers()
    {
        var answ = await _statisticService.GetHotOffersAsync();
        return Ok(answ);
    }

    [HttpGet("FinanceAnalitics")]
    public async Task<IActionResult> GetFinanceAnalitics()
    {
        var answ = await _statisticService.GetFinanceAnalyticsAsync();
        return Ok(answ);
    }

    [HttpGet("DecommisStock")]
    public async Task<IActionResult> GetDecommisStock()
    {
        var answ = await _statisticService.GetDecommissionedStockAsync();
        return Ok(answ);
    }

    [HttpGet("TopDishes")]
    public async Task<IActionResult> GetTopDishes()
    {
        var answ = await _statisticService.GetTopDishesAsync();
        return Ok(answ);
    }

    [HttpGet("forecast/stoplist")]
    public async Task<IActionResult> GetStopList()
    {
        var answ = await _forecastService.GetStopListAsync();
        return Ok(answ);
    }

    [HttpGet("forecast/deliveryRecomend")]
    public async Task<IActionResult> GetDeliveryRecommendations()
    {
        var answ = await _forecastService.GetDeliveryRecommendationsAsync();
        return Ok(answ);
    }
}
