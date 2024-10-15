using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzalandCore.Models;

namespace PizzalandCore.Db.Configuration;

public class PizzasConfiguration : IEntityTypeConfiguration<Pizza>
{
    public void Configure(EntityTypeBuilder<Pizza> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(50);
        builder.Property(p => p.Price).HasColumnType("decimal(5,3)").HasPrecision(3);
        builder.Property(p => p.Ingredients).HasMaxLength(200);
    }
}
