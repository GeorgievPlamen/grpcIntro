using Grpc.Core;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Services;

public class PizzasService(IPizzaRepository pizzaRepository) : PizzaService.PizzaServiceBase
{
    private readonly IPizzaRepository _pizzaRepository = pizzaRepository;

    public async override Task<PizzaResponse> CreatePizza(CreatePizzaRequest request, ServerCallContext context)
    {
        Pizza newPizza = new(
            request.Name,
            request.CrustType,
            request.Ingredients.ToArray(),
            Convert.ToDecimal(request.Price));

        var response = await _pizzaRepository.AddPizzaAsync(newPizza);

        var responsePizza = MapToPizzaResponse(response);

        return new PizzaResponse { Pizza = responsePizza };
    }

    public async override Task<DeletePizzaResponse> DeletePizza(DeletePizzaRequest request, ServerCallContext context)
    {
        var result = await _pizzaRepository.DeletePizzaAsync(Guid.Parse(request.Id));

        var responseMsg =
            result ?
            $"Deleted pizza with id:{request.Id}." :
            $"Failed to delete or could not find pizza with id: {request.Id}.";

        return new DeletePizzaResponse { Message = responseMsg };
    }

    public async override Task<PizzaResponse> UpdatePizza(UpdatePizzaRequest request, ServerCallContext context)
    {
        Pizza newPizza = new(
            Guid.Parse(request.Pizza.Id),
            request.Pizza.Name,
            request.Pizza.CrustType,
            request.Pizza.Ingredients.ToArray(),
            Convert.ToDecimal(request.Pizza.Price));

        var response = await _pizzaRepository.UpdatePizzaAsync(newPizza);

        var responsePizza = MapToPizzaResponse(response!);

        return new PizzaResponse { Pizza = responsePizza };
    }

    public async override Task<PizzaResponse?> GetPizza(GetPizzaRequest request, ServerCallContext context)
    {
        var foundPizza = await _pizzaRepository.GetPizzaAsync(Guid.Parse(request.Id));
        if (foundPizza is null) return null;
        var responsePizza = MapToPizzaResponse(foundPizza);
        return new PizzaResponse { Pizza = responsePizza };
    }

    public async override Task<ListPizzasResponse> ListPizzas(ListPizzasRequest request, ServerCallContext context)
    {
        var pizzas = await _pizzaRepository.GetPizzasAsync();

        var pizzaResponses = pizzas.Select(MapToPizzaResponse).ToList();

        return new ListPizzasResponse
        {
            Pizzas = { pizzaResponses }
        };
    }

    private static PizzaProto MapToPizzaResponse(Pizza pizza) => new PizzaProto
    {
        Id = pizza.Id.ToString(),
        Name = pizza.Name,
        CrustType = pizza.CrustType,
        Ingredients = { pizza.Ingredients },
        Price = (double)pizza.Price
    };
}
