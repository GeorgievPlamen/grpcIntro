using System.ComponentModel.DataAnnotations;

namespace PizzalandCore.Models;

public class User
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(5)]
    public string Password { get; set; } = null!;
    public DateTime DateRegistered { get; init; }
    public bool IsActive { get; private set; } = true;
    public void Deactivate() => IsActive = false;
    public string Token { get; set; } = "";
}