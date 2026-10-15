using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IStatisticService _statisticService;

    public DashboardController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
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
}
