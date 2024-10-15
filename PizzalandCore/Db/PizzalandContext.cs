using Microsoft.EntityFrameworkCore;
using PizzalandCore.Models;

namespace PizzalandCore.Db;

public class PizzalandContext : DbContext
{
    public PizzalandContext(DbContextOptions<PizzalandContext> options) : base(options)
    {
    }

    public DbSet<Pizza> Pizzas { get; set; }
}
