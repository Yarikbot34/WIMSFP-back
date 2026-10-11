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
            return BadRequest(ex.Message);
        }
    }
}