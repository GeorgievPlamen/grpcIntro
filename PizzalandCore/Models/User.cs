namespace PizzalandCore.Models;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime DateRegistered { get; init; }
    public bool IsActive { get; set; }
}