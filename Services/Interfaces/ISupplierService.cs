using Domain.DTO;

namespace Services.Interfaces;

public interface ISupplierService
{
    Task<List<SupplierDto>> GetAllSuppliersAsync();

    Task AddSupplierAsync(SupplierDto supplier);
    Task UpdateSupplierAsync(SupplierDto supplier);
}
