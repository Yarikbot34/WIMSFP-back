using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpPost("AddDish")]
    public async Task<IActionResult> AddDishAsync([FromBody] DishBigDto dto)
    {
        try
        {
            await _dishService.AddDishAsync(dto);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("UpdateDish")]
    public async Task<IActionResult> UpdateDishAsync([FromBody] DishBigDto dto)
    {
        try
        {
            await _dishService.UpdateDishAsync(dto);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ToggleAvailability")]
    public async Task<IActionResult> ToggleDishAvailabilityAsync([FromBody] string name)
    {
        try
        {
            await _dishService.ToggleDishAvailabilityAsync(name);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
