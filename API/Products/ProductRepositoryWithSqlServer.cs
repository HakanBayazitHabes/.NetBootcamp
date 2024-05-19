using API.Repositories;

namespace API.Products;

public class ProductRepositoryWithSqlServer(AppDbContext context) : IProductRepository
{

    public IReadOnlyList<Product> GetAll()
    {
        return context.Products.ToList().AsReadOnly();
    }

    public IReadOnlyList<Product> GetAllByPage(int page, int pageSize)
    {
        return context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList().AsReadOnly();
    }

    public void Create(Product product)
    {
        context.Products.Add(product);
    }

    public void UpdateProductName(string name, int id)
    {
        var product = GetById(id);
        product!.Name = name;
        context.Products.Update(product);
    }

    public void Delete(int id)
    {
        var product = GetById(id);
        if (product is null)
            return;

        context.Products.Remove(product);
    }

    public Product? GetById(int id)
    {
        return context.Products.Find(id);
    }

    public bool IsExists(string productName)
    {
        return context.Products.Any(x => x.Name == productName);
    }

    public void Update(Product product)
    {
        context.Products.Update(product);
    }
}
