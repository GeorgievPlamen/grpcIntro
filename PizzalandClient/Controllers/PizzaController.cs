using Microsoft.AspNetCore.Mvc;
using pizzalandClient.Interfaces;
using pizzalandClient.Models;

namespace pizzalandClient.Controllers;

public class PizzaController(ITokenProvider tokenProvider, ILogger<PizzaController> logger, PizzaService.PizzaServiceClient pizzaClient, OrderService.OrderServiceClient orderClient) : Controller
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ILogger<PizzaController> _logger = logger;
    private readonly PizzaService.PizzaServiceClient _pizzaClient = pizzaClient;
    private readonly OrderService.OrderServiceClient _orderClient = orderClient;

    [HttpGet]
    public async Task<ActionResult> ListPizzas()
    {
        var response = await _pizzaClient.ListPizzasAsync(new ListPizzasRequest());

        _logger.LogInformation(_tokenProvider.GetToken(CancellationToken.None));

        var pizzas = response.Pizzas.Select(p => new PizzaViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Ingredients = string.Join(",", p.Ingredients),
            Price = p.Price
        }).ToList();

        return View(pizzas);
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(List<string> selectedPizzaIds)
    {
        var userId = _tokenProvider.GetUserId();

        var orderRequest = new CreateOrderRequest
        {
            PizzaIds = { selectedPizzaIds },
            UserId = userId
        };


        var response = await _orderClient.CreateOrderAsync(orderRequest);

        if (response.Order != null)
        {
            return RedirectToAction("ManageOrders", "Order");

        }
        else
        {
            ModelState.AddModelError("", "Failed to create order.");
            return RedirectToAction("ListPizzas");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ManagePizzas()
    {
        var response = await _pizzaClient.ListPizzasAsync(new ListPizzasRequest());
        var pizzas = response.Pizzas.Select(p => new CreatePizzaViewModel
        {
            Name = p.Name,
            CrustType = (pizzalandClient.Models.PizzaCrust)p.CrustType,
            Ingredients = p.Ingredients.ToList(),
            Price = p.Price
        }).ToList();

        ViewBag.Pizzas = pizzas;
        return View(new CreatePizzaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePizza(CreatePizzaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = await _pizzaClient.ListPizzasAsync(new ListPizzasRequest());
            ViewBag.Pizzas = response.Pizzas.Select(p => new CreatePizzaViewModel
            {
                Name = p.Name,
                CrustType = (Models.PizzaCrust)p.CrustType,
                Ingredients = p.Ingredients.ToList(),
                Price = p.Price
            }).ToList();

            return View("ManagePizzas", model);
        }

        var token = _tokenProvider.GetToken(CancellationToken.None);
        if (string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "You must be logged in to create a pizza.";
            return RedirectToAction("Login", "User");
        }

        var createPizzaRequest = new CreatePizzaRequest
        {
            Name = model.Name,
            CrustType = (pizzalandClient.PizzaCrust)model.CrustType,
            Ingredients = { model.Ingredients },
            Price = model.Price
        };

        await _pizzaClient.CreatePizzaAsync(createPizzaRequest);

        return RedirectToAction("ManagePizzas");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePizza(string id)
    {
        await _pizzaClient.DeletePizzaAsync(new DeletePizzaRequest { Id = id });
        return RedirectToAction("ManagePizzas");
    }
}
