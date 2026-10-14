using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/Stock")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("Sum")]
    public async Task<IActionResult> Get()
    {
        var answ = await _stockService.GetGroupedStocksAsync();
        return Ok(answ);
    }

    [HttpGet("getExpiredSoon")]
    public async Task<IActionResult> GetExpiredSoon()
    {
        var answ = await _stockService.GetExpiredSoonAsync();
        return Ok(answ);
    }

    [HttpGet("getExpired")]
    public async Task<IActionResult> GetExpired()
    {
        var answ = await _stockService.GetExpiredAsync();
        return Ok(answ);
    }
}