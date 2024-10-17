using System.ComponentModel.DataAnnotations;
namespace pizzalandClient.Models;

public class CreatePizzaViewModel
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please select a crust type")]
    public PizzaCrust CrustType { get; set; }

    [Required(ErrorMessage = "At least one ingredient is required")]
    public List<string> Ingredients { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price")]
    public double Price { get; set; }
}

public enum PizzaCrust
{
    ITALIAN = 0,
    THIN = 1,
    MEDIUM = 2,
    THICK = 3,
    STUFFED = 4
}