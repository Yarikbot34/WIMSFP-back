using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using WIMSFP.Hubs;

namespace WIMSFP.API;

[Route("api/v1/supply")]
public class SupplyController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;
    private readonly IHubContext<DashboardHub> _hubContext;

    public SupplyController(IDeliveryService deliveryService, IHubContext<DashboardHub> hubContext)
    {
        _deliveryService = deliveryService;
        _hubContext = hubContext;
    }

    [HttpPost("AddDelivery")]
    public async Task<IActionResult> WriteDeliveryAsync([FromBody]DeliveryDto dto)
    {
        try
        {
            if (dto.DateReceived > DateTime.UtcNow)
                return BadRequest("Delivery date cannot be in the future");

            await _deliveryService.WriteDeliveryAsync(dto);
            await DashboardHub.NotifyDataChangedAsync(_hubContext, "New delivery received");
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