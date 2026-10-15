using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WIMSFP.API;

[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var answ = await _userService.GetUsersAsync();
            return Ok(answ);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        try
        {
            await _userService.CreateUserAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _userService.LoginAsync(dto);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("deleteUser/{name}")]
    public async Task<IActionResult> DeleteUser(string name)
    {
        try
        {
            await _userService.DeleteUserAsync(name);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
