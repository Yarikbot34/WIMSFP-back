using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userR;
    private readonly ITokenCreator _tokenCreator;

    public UserService(IUserRepo userR, ITokenCreator tokenCreator)
    {
        _userR = userR;
        _tokenCreator = tokenCreator;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userR.GetAllAsync();
        return users.Select(ToDto).ToList();
    }

    public async Task CreateUserAsync(CreateUserDto dto)
    {
        var existing = await _userR.GetByNameAsync(dto.Name);
        if (existing is not null) throw new Exception("User already exists");

        if (!Enum.TryParse<User.UserRole>(dto.Role, ignoreCase: true, out var role))
            throw new Exception("Unknown user role");

        User user = new User
        {
            Name = dto.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };
        await _userR.CreateAsync(user);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userR.GetByNameAsync(dto.Name);
        if (user is null) throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid password");

        return _tokenCreator.CreateToken(user);
    }

    public async Task DeleteUserAsync(string name)
    {
        var user = await _userR.GetByNameAsync(name);
        if (user is null) throw new Exception("User not found");

        await _userR.DeleteAsync(user);
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Name = user.Name,
            Role = user.Role.ToString()
        };
    }
}
