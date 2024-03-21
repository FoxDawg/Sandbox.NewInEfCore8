using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.ComplexTypes.Model;

public record RecordPrice(decimal Amount, Currency Currency);

public class RecordPriceProduct
{
    private RecordPriceProduct()
    {
    }

    public int Id { get; }
    public string Name { get; }

    public required RecordPrice RegularPrice { get; set; }
    public required RecordPrice RetailPrice { get; set; }

    public RecordPriceProduct(string name)
    {
        Name = name;
    }
}

public class RecordPriceProductConfiguration : IEntityTypeConfiguration<RecordPriceProduct>
{
    public void Configure(EntityTypeBuilder<RecordPriceProduct> builder)
    {
        builder.ToTable("RecordPriceProducts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
        builder.ComplexProperty(p => p.RegularPrice);
        builder.ComplexProperty(p => p.RetailPrice);
    }
}