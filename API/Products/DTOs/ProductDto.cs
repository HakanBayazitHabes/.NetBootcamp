namespace API.Products.DTOs;

public record ProductDto(int Id, string Name, decimal Price, string CreatedDate);

// public record ProductDto
// {
//     public int Id { get; init; }
//     public string Name { get; init; } = default!;
//     public decimal Price { get; init; }
//     public string CreatedDate { get; init; } = default!;
// }
