using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.ComplexTypes.Model;

public class ImmutablePrice
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private ImmutablePrice()
    {
    }

    public ImmutablePrice(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

public class ImmutablePriceProduct
{
    private ImmutablePriceProduct()
    {
    }

    public int Id { get; }
    public string Name { get; }

    public required ImmutablePrice RegularPrice { get; set; }
    public required ImmutablePrice RetailPrice { get; set; }

    public ImmutablePriceProduct(string name)
    {
        Name = name;
    }
}

public class ImmutablePriceProductConfiguration : IEntityTypeConfiguration<ImmutablePriceProduct>
{
    public void Configure(EntityTypeBuilder<ImmutablePriceProduct> builder)
    {
        builder.ToTable("ImmutablePriceProducts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
        builder.ComplexProperty(p => p.RegularPrice, b =>
        {
            b.Property(o => o.Amount);
            b.Property(o => o.Currency);
        });
        builder.ComplexProperty(p => p.RetailPrice, b =>
        {
            b.Property(o => o.Amount);
            b.Property(o => o.Currency);
        });
    }
}