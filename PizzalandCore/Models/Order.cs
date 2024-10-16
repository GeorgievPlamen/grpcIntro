namespace PizzalandCore.Models;

public class Order
{
    private const decimal _deliveryCost = 20;
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime TimeOfOrder { get; init; } = DateTime.UtcNow;
    public List<Guid> PizzaIdsOrdered { get; set; } = [];
    public decimal TotalPrice => IsDeliveryCovered ? Price : Price + _deliveryCost;
    public decimal Price { get; set; }
    internal bool IsDeliveryCovered => Price > 100;
}