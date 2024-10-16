using PizzalandCore.Models;

namespace PizzalandCore.Interfaces;

public interface IPizzaRepository
{
    Task<Pizza?> GetPizzaAsync(Guid id);
    Task<List<Pizza>> GetPizzasAsync();
    Task<List<Pizza>> GetPizzasByIdsAsync(List<Guid> ids);
    Task<Pizza> AddPizzaAsync(Pizza pizza);
    Task<bool> DeletePizzaAsync(Guid id);
    Task<Pizza?> UpdatePizzaAsync(Pizza pizza);
}