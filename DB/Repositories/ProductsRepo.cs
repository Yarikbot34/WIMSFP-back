using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class ProductsRepo : IProductsRepo
{
    private readonly AppDbContext _dbContext;
    
    public ProductsRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var answ = await _dbContext.Products.ToListAsync();
        return answ;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Products.FindAsync(id);
        return answ;
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        var answ = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
        return answ;
    }


    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
    
}