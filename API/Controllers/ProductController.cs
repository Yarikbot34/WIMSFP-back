using Domain.Class;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/Products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductList()
    {
        var answ = await _productService.GetAllProductsAsync();
        return Ok(answ);
    }

    [HttpGet("Units")]
    public async Task<IActionResult> GetUnitsList()
    {
        return Ok(Enum.GetNames(typeof(Product.Unit)).ToList());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
    {
        try
        {
            await _productService.AddProductAsync(dto);
            return Ok();
        } catch  (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Remove")]
    public async Task<IActionResult> RemoveProduct([FromBody] ProductDto dto)
    {
        try
        {
            await _productService.DeleteProductAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}