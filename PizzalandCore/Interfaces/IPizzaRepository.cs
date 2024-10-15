using PizzalandCore.Models;

namespace PizzalandCore.Interfaces;

public interface IPizzaRepository
{
    Pizza? GetPizzaAsync(Guid id);
    List<Pizza> GetPizzasAsync();
    Pizza AddPizzaAsync(Pizza pizza);
    bool DeletePizzaAsync(Guid id);
    Pizza? UpdatePizzaAsync(Pizza pizza);
}