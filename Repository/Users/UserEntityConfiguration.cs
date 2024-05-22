using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Repository.Users;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Name).IsRequired().HasMaxLength(100);
        builder.Property(user => user.Email).IsRequired().HasMaxLength(100);
        builder.Property(user => user.Password).IsRequired().HasMaxLength(100);
        builder.Property(user => user.Age).IsRequired();
        builder.HasCheckConstraint("CK_User_Age", "[Age] >= 0 AND [Age] <= 100");
        builder.Property(user => user.CreatedDate).IsRequired();
        builder.HasMany(user => user.Roles).WithMany(role => role.Users);
    }
}
