using System.ComponentModel.DataAnnotations;

namespace pizzalandClient.Models;

public class Order
{
    private const decimal _deliveryCost = 20;
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public DateTime TimeOfOrder { get; init; } = DateTime.UtcNow;
    public List<Guid> PizzaIdsOrdered { get; set; } = [];
    public decimal TotalPrice => IsDeliveryCovered ? Price : Price + _deliveryCost;
    public decimal Price { get; set; }
    internal bool IsDeliveryCovered => Price > 100;
}