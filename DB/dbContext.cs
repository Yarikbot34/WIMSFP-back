using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class AppDbContext : DbContext
{
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public AppDbContext()
    {
        Database.EnsureCreated();
    }
}