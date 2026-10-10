using Domain.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasAlternateKey(p => p.Name);
        });

        modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasOne(s => s.Product)
                    .WithMany()
                    .HasForeignKey(s => s.ProductId);
                entity.HasOne(s => s.Delivery)
                    .WithMany()
                    .HasForeignKey(s => s.DeliveryId);
                entity.ToTable("Stocks", table =>
                {
                    table.HasCheckConstraint("ST_Count", "Count > 0");
                });
            }
        );

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasAlternateKey(d => d.Number);
        });
    }
}