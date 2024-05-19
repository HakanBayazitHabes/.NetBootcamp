using API.Repositories;

namespace API.Products.AsyncMethods;

public interface IProductRepository2 : IGenericRepository<Product>
{
    Task UpdateProductName(string name, int id);
}
