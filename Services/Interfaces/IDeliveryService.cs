using Domain.DTO;

namespace Services.Interfaces;

public interface IDeliveryService
{
    Task WriteDeliveryAsync(DeliveryDto dto);
    Task<List<DeliveryDto>> GetDeliveriesAsync();
    Task<DeliveryDto> GetDeliveryByNumberAsync(string number);
    Task<int> DeliveryReceivedByMonthAsync();
}