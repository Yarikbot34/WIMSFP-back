using Domain.DTO;

namespace Services.Interfaces;

public interface IDeliveryService
{
    Task WriteDeliveryAsync(DeliveryDto dto);
}