using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class SupplierRepo : ISupplierRepo
{
    private readonly AppDbContext _dbContext;

    public SupplierRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        var answ = await _dbContext.Suppliers.ToListAsync();
        return answ;
    }

    public async Task<Supplier?> GetByNameAsync(string name)
    {
        var answ = await _dbContext.Suppliers
            .FirstOrDefaultAsync(s => s.Name == name);
        return answ;
    }

    public async Task CreateAsync(Supplier supplier)
    {
        _dbContext.Suppliers.Add(supplier);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _dbContext.Suppliers.Update(supplier);
        await _dbContext.SaveChangesAsync();
    }
}
