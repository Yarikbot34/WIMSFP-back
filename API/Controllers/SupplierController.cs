using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/Suppliers")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSupplierList()
    {
        var answ = await _supplierService.GetAllSuppliersAsync();
        return Ok(answ);
    }

    [HttpPost("AddSupplier")]
    public async Task<IActionResult> AddSupplier([FromBody] SupplierDto dto)
    {
        try
        {
            await _supplierService.AddSupplierAsync(dto);
            return Ok();
        } catch  (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("UpdateSupplier")]
    public async Task<IActionResult> UpdateSupplier([FromBody] SupplierDto dto)
    {
        try
        {
            await _supplierService.UpdateSupplierAsync(dto);
            return Ok();
        } catch  (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
