using Domain.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DB;

public class AppDbContext : DbContext
{
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishIngredient> DishIngredients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderObj> OrderObjs { get; set; }
    
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
                    table.HasCheckConstraint("ST_Count", "\"Count\" > 0");
                });
            }
        );

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasAlternateKey(d => d.Number);
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasAlternateKey(d => d.Name);
            entity.HasMany(d => d.Ingredients)
                .WithOne()
                .HasForeignKey(d => d.DishId);
        });

        modelBuilder.Entity<DishIngredient>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId);
                entity.HasOne(d => d.Dish)
                    .WithOne()
                    .HasForeignKey<DishIngredient>(d => d.DishId);
                
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("DI_Count", "\"Amount\" > 0");
                });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.HasMany(o => o.Objs)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);
        });

        modelBuilder.Entity<OrderObj>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.HasOne(o => o.Order)
                .WithMany(or => or.Objs)
                .HasForeignKey(o => o.OrderId);
            entity.HasOne(o => o.Dish)
                .WithMany()
                .HasForeignKey(o => o.DishId);
        });
    }
}