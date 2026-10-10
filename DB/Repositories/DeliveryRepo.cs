using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class DeliveryRepo : IDeliveryRepo
{
    private readonly AppDbContext _dbContext;

    public DeliveryRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Delivery>> GetAllAsync()
    {
        var answ = await _dbContext.Deliveries.ToListAsync();
        return answ;
    }

    public async Task<Delivery?> GetByIdAsync(int id)
    {
        var answ = await _dbContext.Deliveries.FindAsync(id);
        return answ;
    }

    public async Task<Delivery?> GetByNumberAsync(string number)
    {
        var answ = await _dbContext.Deliveries
            .FirstOrDefaultAsync(d => d.Number == number);
        return answ;
    }

    public async Task<List<Delivery>> GetByDateAsync(DateOnly date)
    {
        var answ = await _dbContext.Deliveries
            .Where(d => DateOnly.FromDateTime(d.DateReceived) == date)
            .ToListAsync();
        return answ;
    }

    public async Task CreateAsync(Delivery delivery)
    {
        _dbContext.Deliveries.Add(delivery);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Delivery delivery)
    {
        _dbContext.Deliveries.Update(delivery);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Delivery delivery)
    {
        _dbContext.Deliveries.Remove(delivery);
        await _dbContext.SaveChangesAsync();
    }
}