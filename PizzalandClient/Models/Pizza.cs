using System.ComponentModel.DataAnnotations;

namespace pizzalandClient.Models;

public class Pizza
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public PizzaCrust CrustType { get; set; }
    public string[] Ingredients { get; set; }
    public decimal Price { get; set; }

    public Pizza(string name, PizzaCrust crustType, string[] ingredients, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        CrustType = crustType;
        Ingredients = ingredients;
        Price = price;
    }
    public Pizza(Guid id, string name, PizzaCrust crustType, string[] ingredients, decimal price)
    {
        Id = id;
        Name = name;
        CrustType = crustType;
        Ingredients = ingredients;
        Price = price;
    }
}