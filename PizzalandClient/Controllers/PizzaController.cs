using Microsoft.AspNetCore.Mvc;
using pizzalandClient;

public class PizzaController : Controller
{
    private readonly PizzaService.PizzaServiceClient _pizzaClient;
    private readonly OrderService.OrderServiceClient _orderClient;

    public PizzaController(PizzaService.PizzaServiceClient pizzaClient, OrderService.OrderServiceClient orderClient)
    {
        _pizzaClient = pizzaClient;
        _orderClient = orderClient;
    }

    [HttpGet]
    public async Task<ActionResult> ListPizzas()
    {
        var response = await _pizzaClient.ListPizzasAsync(new ListPizzasRequest());

        var pizzas = response.Pizzas.Select(p => new PizzaViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Ingredients = string.Join(", ", p.Ingredients),
            Price = p.Price
        }).ToList();

        return View(pizzas);
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(List<string> selectedPizzaIds)
    {
        var userId = HttpContext.Session.GetString("UserId"); // assuming user id is stored
        var orderRequest = new CreateOrderRequest
        {
            PizzaIds = { selectedPizzaIds },
            UserId = userId
        };

        var response = await _orderClient.CreateOrderAsync(orderRequest);

        if (response.Order != null)
        {
            return RedirectToAction("OrderSuccess");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create order.");
            return RedirectToAction("ListPizzas");
        }
    }
}
