using System.Threading.Tasks;
using Demos.ComplexTypes.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Demos.ComplexTypes;

public class ComplexTypeDemos
{
    private readonly ITestOutputHelper output;

    public ComplexTypeDemos(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public async Task TrackingMutablePrices_LeadsToIssues()
    {
        // Add product
        await using var writeDbContext = DemoDbContext.Create(output: output);

        var productPrice = new MutablePrice(100, Currency.USD);
        writeDbContext.Set<MutablePriceProduct>().Add(new MutablePriceProduct("ProductA")
        {
            RegularPrice = productPrice,
            RetailPrice = productPrice
        });
        await writeDbContext.SaveChangesAsync();

        // Change one of the prices on product
        var productToModify = await writeDbContext.Set<MutablePriceProduct>().FirstAsync();
        productToModify.RegularPrice.Amount = 250;
        await writeDbContext.SaveChangesAsync();

        // Assert product
        await using var readContext = DemoDbContext.Create();
        var resultingProduct = await readContext.Set<MutablePriceProduct>().FirstAsync();
        resultingProduct.RegularPrice.Amount.Should().Be(250m);
        resultingProduct.RetailPrice.Amount.Should().Be(250m);
    }
    
    [Fact]
    public async Task TrackingImmutablePrices_LeadsToExpectedResults()
    {
        // Add product
        await using var writeDbContext = DemoDbContext.Create(output: output);

        var productPrice = new ImmutablePrice(100, Currency.USD);
        writeDbContext.Set<ImmutablePriceProduct>().Add(new ImmutablePriceProduct("ProductA")
        {
            RegularPrice = productPrice,
            RetailPrice = productPrice
        });
        await writeDbContext.SaveChangesAsync();

        // Change one of the prices on product
        var productToModify = await writeDbContext.Set<ImmutablePriceProduct>().FirstAsync();
        productToModify.RegularPrice = new ImmutablePrice(250, Currency.USD);
        await writeDbContext.SaveChangesAsync();

        // Assert product
        await using var readContext = DemoDbContext.Create();
        var resultingProduct = await readContext.Set<ImmutablePriceProduct>().FirstAsync();
        resultingProduct.RegularPrice.Amount.Should().Be(250m);
        resultingProduct.RetailPrice.Amount.Should().Be(100m);
    }
    
    [Fact]
    public async Task TrackingRecordPrices_LeadsToExpectedResults()
    {
        // Add product
        await using var writeDbContext = DemoDbContext.Create(output: output);

        var productPrice = new RecordPrice(100, Currency.USD);
        writeDbContext.Set<RecordPriceProduct>().Add(new RecordPriceProduct("ProductA")
        {
            RegularPrice = productPrice,
            RetailPrice = productPrice
        });
        await writeDbContext.SaveChangesAsync();

        // Change one of the prices on product
        var productToModify = await writeDbContext.Set<RecordPriceProduct>().FirstAsync();
        productToModify.RegularPrice = new RecordPrice(250, Currency.USD);
        await writeDbContext.SaveChangesAsync();

        // Assert product
        await using var readContext = DemoDbContext.Create();
        var resultingProduct = await readContext.Set<RecordPriceProduct>().FirstAsync();
        resultingProduct.RegularPrice.Amount.Should().Be(250m);
        resultingProduct.RetailPrice.Amount.Should().Be(100m);
    }
}