namespace PizzalandCore.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime TimeOfOrder { get; init; } = DateTime.UtcNow;
    public List<Pizza> PizzasOrdered { get; set; } = [];
    public decimal TotalPrice => PizzasOrdered.Sum(x => x.Price);
    internal bool IsDeliveryCovered => TotalPrice > 100;
}