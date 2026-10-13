using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.DTO;
using Services.Interfaces;

namespace Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepo _supplierR;

    public SupplierService(ISupplierRepo supplierR)
    {
        _supplierR = supplierR;
    }

    public async Task<List<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierR.GetAllAsync();
        return suppliers.Select(ToDto).ToList();
    }

    public async Task AddSupplierAsync(SupplierDto supplier)
    {
        var existing = await _supplierR.GetByNameAsync(supplier.Name);
        if (existing is not null) throw new Exception("Supplier already exists");

        Supplier sp = new Supplier
        {
            Name = supplier.Name,
            Email = supplier.Email
        };
        await _supplierR.CreateAsync(sp);
    }

    public async Task UpdateSupplierAsync(SupplierDto supplier)
    {
        var sp = await _supplierR.GetByNameAsync(supplier.Name);
        if (sp is null) throw new Exception("Supplier not found");

        sp.Name = supplier.Name;
        sp.Email = supplier.Email;
        await _supplierR.UpdateAsync(sp);
    }

    private static SupplierDto ToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Name = supplier.Name,
            Email = supplier.Email
        };
    }
}
