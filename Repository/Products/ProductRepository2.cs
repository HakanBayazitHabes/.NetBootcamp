using Repository;
using Repository.Products;
using Repository.Products.AsyncMethods;

namespace Repository.Products.AsyncMethods;

public class ProductRepository2(AppDbContext context) : GenericRepository<Product>(context), IProductRepository2
{
    public async Task UpdateProductName(string name, int id)
    {
        var product = await GetById(id);
        product!.Name = name;
        await Update(product);
    }
}
