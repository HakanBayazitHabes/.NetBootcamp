namespace API.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Barcode { get; init; } = default!;
        public int Stock { get; set; }
    }
}