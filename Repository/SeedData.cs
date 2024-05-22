using Repository.Categories;
using Repository.Roles;

namespace Repository;

public class SeedData
{
    public static void SeedDatabase(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            var categories = new[]
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics" },
                new Category { Id = Guid.NewGuid(), Name = "Clothing" },
                new Category { Id = Guid.NewGuid(), Name = "Grocery" },
                new Category { Id = Guid.NewGuid(), Name = "Books" },
                new Category { Id = Guid.NewGuid(), Name = "Furniture" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

        }

        if (!context.Roles.Any())
        {
            var roles = new[]
            {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" },
            new Role { Id = 3, Name = "Moderator" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

        }




    }

}
