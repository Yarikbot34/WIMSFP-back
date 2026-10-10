using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IProductsRepo
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByNameAsync(string name);
    
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    
    Task DeleteAsync(Product product);
}