using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using pizzalandClient;
using pizzalandClient.Interfaces;
using pizzalandClient.Models;

public class OrderController(OrderService.OrderServiceClient orderClient, ITokenProvider tokenProvider, ILogger<OrderController> logger) : Controller
{
    private readonly OrderService.OrderServiceClient _orderClient = orderClient;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ILogger<OrderController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> ManageOrders()
    {
        try
        {
            var response = await _orderClient.ListOrdersAsync(new ListOrdersRequest());
            var orders = response.Orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                PizzaIds = o.PizzaIdsOrdered.Split(',').ToList(),
                TimeOfOrder = o.TimeOfOrder.ToDateTime(),
                TotalPrice = o.TotalPrice,
                IsDeliveryCovered = o.IsDeliveryCovored
            }).ToList();

            return View(orders);
        }
        catch (RpcException ex)
        {
            _logger.LogError($"An error occurred while fetching orders: {ex.Message}");
            TempData["ErrorMessage"] = "Failed to load orders. Please try again later.";
            return View(new List<OrderViewModel>());
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(OrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ManageOrders");
        }

        var token = _tokenProvider.GetToken(CancellationToken.None);
        if (string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "You must be logged in to create an order.";
            return RedirectToAction("Login", "User");
        }

        try
        {
            var createOrderRequest = new CreateOrderRequest
            {
                PizzaIds = { model.PizzaIds },
                UserId = model.UserId
            };

            await _orderClient.CreateOrderAsync(createOrderRequest);
            return RedirectToAction("ManageOrders");
        }
        catch (RpcException ex)
        {
            _logger.LogError($"An error occurred while creating the order: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while creating the order. Please try again later.";
            return RedirectToAction("ManageOrders");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrder(OrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid order data.";
            return RedirectToAction("ManageOrders");
        }

        try
        {
            var updateOrderRequest = new UpdateOrderRequest
            {
                Order = new OrderProto
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    PizzaIdsOrdered = string.Join(",", model.PizzaIds),
                    TimeOfOrder = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(model.TimeOfOrder.ToUniversalTime()),
                    TotalPrice = model.TotalPrice,
                    IsDeliveryCovored = model.IsDeliveryCovered
                }
            };

            await _orderClient.UpdateOrderAsync(updateOrderRequest);
            return RedirectToAction("ManageOrders");
        }
        catch (RpcException ex)
        {
            _logger.LogError($"An error occurred while updating the order: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while updating the order. Please try again later.";
            return RedirectToAction("ManageOrders");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        try
        {
            await _orderClient.DeleteOrderAsync(new DeleteOrderRequest { Id = id });
            return RedirectToAction("ManageOrders");
        }
        catch (RpcException ex)
        {
            _logger.LogError($"An error occurred while deleting the order: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while deleting the order. Please try again later.";
            return RedirectToAction("ManageOrders");
        }
    }
}
