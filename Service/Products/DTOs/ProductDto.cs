namespace Service.Products.DTOs;

public class ProductDto
{
    public ProductDto()
    {

    }

    public int Id { get; set; }
        public string Name { get; set; } = default!;
    public decimal Price { get; set; }
        public string CreatedDate { get; set; } = default!;

    public ProductDto(int id, string name, decimal price, string created)
    {
        Id = id;
        Name = name;
        Price = price;
        CreatedDate = created;
    }
}
