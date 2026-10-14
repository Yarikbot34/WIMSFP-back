using Domain.DTO;

namespace Services.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(CreateUserDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task DeleteUserAsync(string name);
}
