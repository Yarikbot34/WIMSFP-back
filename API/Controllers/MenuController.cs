using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/menu")]
public class MenuController : ControllerBase
{
    private readonly IDishService _dishService;

    public MenuController(IDishService dishService)
    {
        _dishService = dishService;
    }


    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        try
        {
            var answ = await _dishService.GetMenuAsync();
            return Ok(answ);
        } catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        
    }
}