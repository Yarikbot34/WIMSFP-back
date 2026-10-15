using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _dbContext;

    public UserRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var answ = await _dbContext.Users.ToListAsync();
        return answ;
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        var answ = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == name);
        return answ;
    }

    public async Task CreateAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}
