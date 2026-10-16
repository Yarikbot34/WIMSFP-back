using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Authorize(Roles = "Admin,Manager,Waiter")]
[Route("api/v1/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDishesAsync()
    {
        var dishes = await _dishService.GetAllDishesAsync();
        return Ok(dishes);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpPost("ToggleActionByAdmin")]
    public async Task<IActionResult> ToggleDishActionByAdminAsync([FromBody] string name)
    {
        try
        {
            await _dishService.ToggleDishActionByAdminAsync(name);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
