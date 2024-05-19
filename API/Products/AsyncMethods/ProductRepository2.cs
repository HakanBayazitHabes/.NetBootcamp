using API.Repositories;

namespace API.Products.AsyncMethods;

public class ProductRepository2(AppDbContext context) : GenericRepository<Product>(context), IProductRepository2
{
    public async Task UpdateProductName(string name, int id)
    {
        var product = await GetById(id);
        product!.Name = name;
        await Update(product);
    }
}
