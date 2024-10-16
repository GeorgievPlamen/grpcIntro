using Microsoft.EntityFrameworkCore;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Db.Repositories;

public class OrderRepository(PizzalandContext dbContext) : IOrderRepository
{
    private readonly PizzalandContext _dbContext = dbContext;

    public async Task<Order> AddOrderAsync(Order Order)
    {
        await _dbContext.AddAsync(Order);
        await _dbContext.SaveChangesAsync();
        return Order;
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var Order = await _dbContext.Orders.FindAsync(id);
        if (Order == null)
        {
            return false;
        }

        _dbContext.Orders.Remove(Order);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<Order?> UpdateOrderAsync(Order Order)
    {
        _dbContext.Orders.Update(Order);

        await _dbContext.SaveChangesAsync();
        return Order;
    }
}
