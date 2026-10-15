using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[Route("api/v1/supply")]
public class SupplyController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public SupplyController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost("AddDelivery")]
    public async Task<IActionResult> WriteDeliveryAsync([FromBody]DeliveryDto dto)
    {
        try
        {
            await _deliveryService.WriteDeliveryAsync(dto);
            return Ok();
        } catch  (Exception ex)
        {
            return BadRequest(ex.StackTrace);
        }
    }

    [HttpGet("GetDeliveries")]
    public async Task<IActionResult> GetDeliveriesAsync()
    {
            var deliveries = await _deliveryService.GetDeliveriesAsync();
            return Ok(deliveries);
        
    }

    [HttpGet("DeliveryReceivedByMonth")]
    public async Task<IActionResult> DeliveryReceivedByMonth()
    {
        var count = await _deliveryService.DeliveryReceivedByMonthAsync();
        return Ok(count);
    }

    [HttpGet("GetDelivery/{number}")]
    public async Task<IActionResult> GetDeliveryByNumberAsync(string number)
    {
        try
        {
            var delivery = await _deliveryService.GetDeliveryByNumberAsync(number);
            return Ok(delivery);
        } catch  (Exception ex)
        {
            return BadRequest(ex.StackTrace);
        }
    }
}