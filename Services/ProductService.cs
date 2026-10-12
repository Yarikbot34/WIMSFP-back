using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class ProductService : IProductService
{
    private readonly IProductsRepo _productsR;

    public ProductService(IProductsRepo productsR)
    {
        _productsR = productsR;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productsR.GetAllAsync();
        return products.Select(ToDto).ToList();
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productsR.GetByIdAsync(id);
        if (product is null) throw new Exception("Product not found");
        return ToDto(product);
    }

    public async Task AddProductAsync(ProductDto product)
    {
        var existing = await _productsR.GetByNameAsync(product.Name);
        if (existing is not null) throw new Exception("Product already exists");

        Product pr = new Product
        {
            Name = product.Name,
            MesUnit = ParseUnit(product.MeasureUnit)
        };
        await _productsR.AddAsync(pr);
    }

    public async Task UpdateProductAsync(ProductDto product)
    {
        var pr = await _productsR.GetByNameAsync(product.Name);
        if (pr is null) throw new Exception("Product not found");

        pr.Name = product.Name;
        pr.MesUnit = ParseUnit(product.MeasureUnit);
        await _productsR.UpdateAsync(pr);
    }

    public async Task DeleteProductAsync(ProductDto product)
    {
        var pr = await _productsR.GetByNameAsync(product.Name);
        if (pr is null) throw new Exception("Product not found");

        await _productsR.DeleteAsync(pr);
    }

    private static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Name = product.Name,
            MeasureUnit = product.MesUnit.ToString()
        };
    }

    private static Product.Unit ParseUnit(string measureUnit)
    {
        if (Enum.TryParse<Product.Unit>(measureUnit, ignoreCase: true, out var unit))
        {
            return unit;
        }
        throw new Exception("Unknown measure unit");
    }
}
