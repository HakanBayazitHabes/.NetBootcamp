namespace API.DTOs;

public record ProductDto
{
    private decimal v1;
    private string v2;

    public ProductDto(int id, string name, decimal v1, string v2)
    {
        Id = id;
        Name = name;
        this.v1 = v1;
        this.v2 = v2;
    }

    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public string CreatedDate { get; init; } = default!;
}
