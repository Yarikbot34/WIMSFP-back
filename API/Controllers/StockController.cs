using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using WIMSFP.Hubs;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/Stock")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IHubContext<DashboardHub> _hubContext;

    public StockController(IStockService stockService, IHubContext<DashboardHub> hubContext)
    {
        _stockService = stockService;
        _hubContext = hubContext;
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

    [HttpGet("GetExpiredStoks")]
    public async Task<IActionResult> GetExpiredStoks()
    {
        var answ = await _stockService.GetExpiredStocksAsync();
        return Ok(answ);
    }

    [HttpDelete("WriteOffStock/{id}")]
    public async Task<IActionResult> WriteOffStock(int id)
    {
        try
        {
            await _stockService.WriteOffStockAsync(id);
            await DashboardHub.NotifyDataChangedAsync(_hubContext, "Stock written off");
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}