using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Services;

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

    public override Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
    {
        return base.DeleteOrder(request, context);
    }

    public override Task<OrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
    {
        return base.UpdateOrder(request, context);
    }

    public override Task<OrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
    {
        return base.GetOrder(request, context);
    }

    public override Task<ListOrdersResponse> ListOrders(ListOrdersRequest request, ServerCallContext context)
    {
        return base.ListOrders(request, context);
    }

    private static OrderProto MapToOrderProto(Order order) => new OrderProto
    {
        Id = order.Id.ToString(),
        IsDeliveryCovored = order.IsDeliveryCovered,
        PizzaIdsOrdered = string.Join(",", order.PizzaIdsOrdered),
        TimeOfOrder = Timestamp.FromDateTime(order.TimeOfOrder),
        TotalPrice = (double)order.TotalPrice,
        UserId = order.UserId.ToString()
    };
}