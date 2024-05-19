namespace API.Products.DTOs;

public class ProductDto
{
    public ProductDto()
    {

    }

    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Created { get; set; }

    public ProductDto(int id, string name, decimal price, string created)
    {
        Id = id;
        Name = name;
        Price = price;
        Created = created;
    }
}
