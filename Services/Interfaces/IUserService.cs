using Domain.DTO;

namespace Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync();
    Task CreateUserAsync(CreateUserDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task DeleteUserAsync(string name);
}
