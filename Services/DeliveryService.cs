using DB.Repositories;
using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepo _deliveryR;
    private readonly IStockRepo _stockR;
    private readonly IProductsRepo _productsR;
    
    public DeliveryService(IDeliveryRepo deliveryR, IStockRepo stockR,  IProductsRepo productsR){
        _deliveryR = deliveryR;
        _stockR = stockR;
        _productsR = productsR;
    }

    public async Task WriteDeliveryAsync(DeliveryDto dto)
    {
        if (dto.ReceivedProducts.Count == 0) throw new Exception("No products received");
        
        Delivery delivery = new Delivery(dto);
        
        var productNames = dto.ReceivedProducts
            .Select(p => p.ProductName)
            .ToList();
        
        var products = await _productsR.GetByNameListAsync(productNames);
        if (products.Count == 0) throw new Exception("No products received");

        List<Stock> stocks = new();
        foreach (var product in dto.ReceivedProducts)
        {
            var pr = products.Find(p => p.Name == product.ProductName);
            if (pr is null) throw new Exception("Product not found");
            Stock st = new Stock()
            {
                Product = pr,
                Delivery =  delivery,
                ExpirationDate = product.ExpirationDate,
                Count = product.Count
            };
        }
        
        await _deliveryR.CreateAsync(delivery);
        await _stockR.CreateRangeAsync(stocks);
    }
}