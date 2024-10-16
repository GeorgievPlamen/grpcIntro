using PizzalandCore.Models;

namespace PizzalandCore.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetOrderAsync(Guid id);
    Task<List<Order>> GetOrdersAsync();
    Task<Order> AddOrderAsync(Order Order);
    Task<bool> DeleteOrderAsync(Guid id);
    Task<Order?> UpdateOrderAsync(Order Order);
}