using Domain.Class;
using Domain.DTO;

namespace Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    
    Task AddProductAsync(ProductDto product);
    Task UpdateProductAsync(ProductDto product);
    
    Task DeleteProductAsync(ProductDto product);
}