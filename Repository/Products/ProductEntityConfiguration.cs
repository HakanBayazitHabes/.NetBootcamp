using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Products;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Name).IsRequired().HasMaxLength(100);
        builder.Property(product => product.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(product => product.CreatedDate).IsRequired();
        builder.Property(product => product.Barcode).IsRequired().HasMaxLength(100);
    }
}
