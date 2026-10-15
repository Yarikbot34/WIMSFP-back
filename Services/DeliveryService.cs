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
    private readonly ISupplierRepo _supplierR;
    
    public DeliveryService(IDeliveryRepo deliveryR, IStockRepo stockR,  IProductsRepo productsR, ISupplierRepo supplierR){
        _deliveryR = deliveryR;
        _stockR = stockR;
        _productsR = productsR;
        _supplierR = supplierR;
    }

    public async Task WriteDeliveryAsync(DeliveryDto dto)
    {
        if (dto.ReceivedProducts.Count == 0) throw new Exception("No products received");
        
        var supplier = await _supplierR.GetByNameAsync(dto.SupplierName);
        if (supplier is null) throw new Exception("Supplier not found");
        
        Delivery delivery = new Delivery(dto);
        delivery.Supplier = supplier;
        
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
                ItemPrice = product.ItemPrice
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
                SupplierName =  delivery.Supplier.Name,
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

    public async Task<int> DeliveryReceivedByMonthAsync()
    {
        var now = DateTime.UtcNow;
        var from = now.AddMonths(-1);
        return await _deliveryR.CountReceivedByDateRangeAsync(from, now);
    }
}
