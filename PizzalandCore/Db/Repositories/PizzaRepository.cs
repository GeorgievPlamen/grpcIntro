using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Db.Repositories;

public class PizzaRepository : IPizzaRepository
{
    private readonly List<Pizza> pizzas = [];

    public Pizza AddPizzaAsync(Pizza pizza)
    {
        pizzas.Add(pizza);

        return pizza;
    }

    public bool DeletePizzaAsync(Guid id)
    {
        var pizza = pizzas.Find(x => x.Id == id);

        if (pizza is null) return false;

        pizzas.Remove(pizza);

        return true;
    }

    public Pizza? GetPizzaAsync(Guid id)
    {
        return pizzas.Find(x => x.Id == id);
    }

    public List<Pizza> GetPizzasAsync()
    {
        return [.. pizzas];
    }

    public Pizza? UpdatePizzaAsync(Pizza pizza)
    {
        var foundPizza = pizzas.Find(x => x.Id == pizza.Id);

        if (foundPizza is null) return null;

        pizzas.Remove(foundPizza);
        pizzas.Add(pizza);
        return pizza;
    }
}
