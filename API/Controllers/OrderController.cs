using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using WIMSFP.Hubs;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IHubContext<DashboardHub> _hubContext;

    public OrderController(IOrderService orderService, IHubContext<DashboardHub> hubContext)
    {
        _orderService = orderService;
        _hubContext = hubContext;
    }

    [HttpPost("makeOrder")]
    public async Task<IActionResult> MakeOrder([FromBody] OrderDto dto)
    {
        try
        {
            var order = await _orderService.MakeOrderAsync(dto);
            await DashboardHub.NotifyDataChangedAsync(_hubContext, "New order received");
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getOrderList")]
    public async Task<IActionResult> GetOrderList()
    {
        var orders = await _orderService.GetOrderListAsync();
        return Ok(orders);
    }
}
