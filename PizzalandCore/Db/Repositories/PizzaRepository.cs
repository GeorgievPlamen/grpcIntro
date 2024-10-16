using Microsoft.EntityFrameworkCore;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Db.Repositories;

public class PizzaRepository(PizzalandContext dbContext) : IPizzaRepository
{
    private readonly PizzalandContext _dbContext = dbContext;

    public async Task<Pizza> AddPizzaAsync(Pizza pizza)
    {
        await _dbContext.AddAsync(pizza);
        await _dbContext.SaveChangesAsync();
        return pizza;
    }

    public async Task<bool> DeletePizzaAsync(Guid id)
    {
        var pizza = await _dbContext.Pizzas.FindAsync(id);
        if (pizza == null)
        {
            return false;
        }

        _dbContext.Pizzas.Remove(pizza);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Pizza?> GetPizzaAsync(Guid id)
    {
        return await _dbContext.Pizzas.FindAsync(id);
    }

    public async Task<List<Pizza>> GetPizzasAsync()
    {
        return await _dbContext.Pizzas.ToListAsync();
    }

    public async Task<List<Pizza>> GetPizzasByIdsAsync(List<Guid> ids)
    {
        return await _dbContext.Pizzas.Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<Pizza?> UpdatePizzaAsync(Pizza pizza)
    {
        _dbContext.Pizzas.Update(pizza);

        await _dbContext.SaveChangesAsync();
        return pizza;
    }
}
