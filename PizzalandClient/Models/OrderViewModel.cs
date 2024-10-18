namespace pizzalandClient.Models
{
    public class OrderViewModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public List<string>? PizzaIds { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public double TotalPrice { get; set; }
        public bool IsDeliveryCovered { get; set; }
    }
}
