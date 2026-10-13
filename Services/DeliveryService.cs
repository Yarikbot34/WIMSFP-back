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
                Count = product.Count,
                InitialCount = product.Count,
                Status = Stock.StockStatus.Active,
            };
            stocks.Add(st);
        }
        
        await _deliveryR.CreateAsync(delivery);
        await _stockR.CreateRangeAsync(stocks);
    }

    public async Task<List<DeliveryDto>> GetDeliveriesAsync()
    {
        var deliveries = await _deliveryR.GetAllAsync();

        List<DeliveryDto> answ = new();
        foreach (var delivery in deliveries)
        {
            var stocks = await _stockR.GetByDeliveryId(delivery.Id);
            answ.Add(new DeliveryDto
            {
                Number = delivery.Number,
                DateReceived = delivery.DateReceived,
                ReceivedProducts = stocks.Select( x => new ResProductDto(x)).ToList()
            });
        }

        return answ;
    }

    public async Task<DeliveryDto> GetDeliveryByNumberAsync(string number)
    {
        var delivery = await _deliveryR.GetByNumberAsync(number);
        if (delivery is null) throw new Exception("Delivery not found");

        var stocks = await _stockR.GetByDeliveryId(delivery.Id);

        return new DeliveryDto
        {
            Number = delivery.Number,
            DateReceived = delivery.DateReceived,
            ReceivedProducts = stocks.Select(x => new ResProductDto(x)).ToList()
            
        };
    }
}
