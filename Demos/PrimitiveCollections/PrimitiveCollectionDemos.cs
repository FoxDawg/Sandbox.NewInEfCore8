using System.Threading.Tasks;
using Demos.PrimitiveCollections.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Demos.PrimitiveCollections;

public class PrimitiveCollectionDemos
{
    private readonly ITestOutputHelper output;

    public PrimitiveCollectionDemos(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    public async Task StringCollection_IsStoredSuccessful()
    {
        await using var writeDbContext = DemoDbContext.Create();
        writeDbContext.Set<Tweet>().Add(new Tweet("myTweet", ["hashtag1", "hashtag2"]));
        await writeDbContext.SaveChangesAsync();

        await using var readDbContext = DemoDbContext.Create();
        var tweet = await readDbContext.Set<Tweet>().FirstAsync();

        using var scope = new AssertionScope();
        tweet.Should().NotBeNull();
        tweet.Tags.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task EnumCollection_IsStoredSuccessful()
    {
        await using var writeDbContext = DemoDbContext.Create();
        writeDbContext.Set<Guest>().Add(new Guest("myTweet", [BreakfastOptions.Tea, BreakfastOptions.Juice]));
        await writeDbContext.SaveChangesAsync();

        await using var readDbContext = DemoDbContext.Create();
        var guest = await readDbContext.Set<Guest>().FirstAsync();

        using var scope = new AssertionScope();
        guest.Should().NotBeNull();
        guest.BreakfastOptions.Should().HaveCount(2);
    }
}