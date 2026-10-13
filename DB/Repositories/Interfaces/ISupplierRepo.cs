using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ISupplierRepo
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByNameAsync(string name);

    Task CreateAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
}
