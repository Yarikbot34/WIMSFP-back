using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("makeOrder")]
    public async Task<IActionResult> MakeOrder([FromBody] OrderDto dto)
    {
        try
        {
            var order = await _orderService.MakeOrderAsync(dto);
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
