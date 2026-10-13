using Domain.DTO;

namespace Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> MakeOrderAsync(OrderDto dto);
    Task<List<OrderDto>> GetOrderListAsync();
}
