using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.ComplexTypes.Model;

public class MutablePrice
{
    public decimal Amount { get; set; }
    public Currency Currency { get; }

    private MutablePrice()
    {
    }

    public MutablePrice(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

public class MutablePriceProduct
{
    private MutablePriceProduct()
    {
    }

    public int Id { get; }
    public string Name { get; }

    public required MutablePrice RegularPrice { get; init; }
    public required MutablePrice RetailPrice { get; init; }

    public MutablePriceProduct(string name)
    {
        Name = name;
    }
}

public class MutablePriceProductConfiguration : IEntityTypeConfiguration<MutablePriceProduct>
{
    public void Configure(EntityTypeBuilder<MutablePriceProduct> builder)
    {
        builder.ToTable("MutablePriceProducts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
        builder.ComplexProperty(p => p.RegularPrice);
        builder.ComplexProperty(p => p.RetailPrice);
    }
}