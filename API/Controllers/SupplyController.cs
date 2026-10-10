using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
namespace WIMSFP.API;

[Route("api/v1/supply")]
public class SupplyController : ControllerBase
{


    [HttpPost("AddDelivery")]
    public async Task<IActionResult> WriteDeliveryAsync(DeliveryDto dto)
    {
        return Ok();
    }
}