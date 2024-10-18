using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Services;

[Authorize]
public class OrdersService(IOrderRepository orderRepository, IPizzaRepository pizzaRepository) : OrderService.OrderServiceBase
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPizzaRepository _pizzaRepository = pizzaRepository;

    public async override Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        var newOrder = new Order
        {
            UserId = Guid.Parse(request.UserId)
        };

        List<Guid> guids = request.PizzaIds.ToArray().Select(x => Guid.Parse(x)).ToList();
        newOrder.PizzaIdsOrdered.AddRange(guids);

        var pizzas = await _pizzaRepository.GetPizzasByIdsAsync(newOrder.PizzaIdsOrdered);

        foreach (var pizza in pizzas)
        {
            newOrder.Price += pizza.Price;
        }

        await _orderRepository.AddOrderAsync(newOrder);

        var orderResponse = MapToOrderProto(newOrder);

        return new OrderResponse { Order = orderResponse };
    }

    public async override Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
    {
        var result = await _orderRepository.DeleteOrderAsync(Guid.Parse(request.Id));

        var responseMsg =
            result ?
            $"Deleted Order with id:{request.Id}." :
            $"Failed to delete or could not find Order with id: {request.Id}.";

        return new DeleteOrderResponse { Message = responseMsg };
    }

    public async override Task<OrderResponse?> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
    {
        var order = await _orderRepository.GetOrderAsync(Guid.Parse(request.Order.Id));

        if (order is null) return null;

        List<Guid> guids = request.Order.PizzaIdsOrdered.Split(',').Select(x => Guid.Parse(x)).ToList();
        order.PizzaIdsOrdered.AddRange(guids);

        var pizzas = await _pizzaRepository.GetPizzasByIdsAsync(order.PizzaIdsOrdered);

        foreach (var pizza in pizzas)
        {
            order.Price += pizza.Price;
        }

        await _orderRepository.AddOrderAsync(order);

        var orderResponse = MapToOrderProto(order);

        return new OrderResponse { Order = orderResponse };
    }

    public async override Task<OrderResponse?> GetOrder(GetOrderRequest request, ServerCallContext context)
    {
        var foundOrder = await _orderRepository.GetOrderAsync(Guid.Parse(request.Id));
        if (foundOrder is null) return null;
        var OrderResponse = MapToOrderProto(foundOrder);
        return new OrderResponse { Order = OrderResponse };
    }

    public async override Task<ListOrdersResponse> ListOrders(ListOrdersRequest request, ServerCallContext context)
    {
        var Order = await _orderRepository.GetOrdersAsync();

        var OrderResponse = Order.Select(MapToOrderProto).ToList();

        return new ListOrdersResponse
        {
            Orders = { OrderResponse }
        };
    }

    public override async Task TrackOrderStatus(TrackOrderRequest request, IServerStreamWriter<OrderStatusUpdate> responseStream, ServerCallContext context)
    {
        var statuses = new[]
        {
            "Your order is accepted",
            "Pizza is in the oven",
            "Pizza is on the way",
            "Pizza is at your door"
        };

        foreach (var status in statuses)
        {
            if (context.CancellationToken.IsCancellationRequested)
                break;

            var update = new OrderStatusUpdate
            {
                Status = status,
                Time = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            await responseStream.WriteAsync(update);

            await Task.Delay(5000);
        }
    }

    private static OrderProto MapToOrderProto(Order order) => new OrderProto
    {
        Id = order.Id.ToString(),
        IsDeliveryCovored = order.IsDeliveryCovered,
        PizzaIdsOrdered = string.Join(",", order.PizzaIdsOrdered),
        TimeOfOrder = Timestamp.FromDateTime(order.TimeOfOrder.ToUniversalTime()),
        TotalPrice = (double)order.TotalPrice,
        UserId = order.UserId.ToString()
    };
}