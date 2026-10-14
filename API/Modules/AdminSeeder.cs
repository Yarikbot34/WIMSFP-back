using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.Extensions.DependencyInjection;

namespace API.BuilderFuctions;

internal static class AdminSeeder
{
    internal static async Task SeedAdminAsync(this IServiceProvider services)
    {
        var login = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return;

        using var scope = services.CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepo>();

        var existing = await userRepo.GetByNameAsync(login);
        if (existing is not null) return;

        User admin = new User
        {
            Name = login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = User.UserRole.Admin
        };
        await userRepo.CreateAsync(admin);
    }
}
