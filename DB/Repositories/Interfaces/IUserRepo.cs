using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IUserRepo
{
    Task<User?> GetByNameAsync(string name);

    Task CreateAsync(User user);
    Task DeleteAsync(User user);
}
