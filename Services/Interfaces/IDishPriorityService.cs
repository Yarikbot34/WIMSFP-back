using Domain.DTO;

namespace Services.Interfaces;

public interface IDishPriorityService
{
    Task RecalculatePrioritiesAsync();
}
