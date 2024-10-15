using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PizzalandCore.Db.Configuration;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).HasMaxLength(100);
        builder.Property(u => u.Name).HasMaxLength(100);
    }
}
