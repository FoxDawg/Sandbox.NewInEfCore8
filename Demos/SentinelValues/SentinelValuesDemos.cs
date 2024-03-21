using System.Threading.Tasks;
using Demos.SentinelValues.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Demos.SentinelValues;

public class SentinelValuesDemos
{
    private readonly ITestOutputHelper output;

    public SentinelValuesDemos(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    public async Task InsertingUsualDefaultValue_StoresThisValue()
    {
        await using var writeDbContext = DemoDbContext.Create(output: output);
        writeDbContext.Set<ToDoItem>().Add(new ToDoItem("shopping"));
        await writeDbContext.SaveChangesAsync();

        await using var readDbContext = DemoDbContext.Create();
        var item = await readDbContext.Set<ToDoItem>().FirstAsync();

        using var scope = new AssertionScope();
        item.Should().NotBeNull();
        item.Priority.Should().Be(0);
    }
    
    [Fact]
    public async Task InsertingRegularValue_StoresThisValue()
    {
        await using var writeDbContext = DemoDbContext.Create(output: output);
        writeDbContext.Set<ToDoItem>().Add(new ToDoItem("shopping")
        {
            Priority = 12
        });
        await writeDbContext.SaveChangesAsync();

        await using var readDbContext = DemoDbContext.Create();
        var item = await readDbContext.Set<ToDoItem>().FirstAsync();

        using var scope = new AssertionScope();
        item.Should().NotBeNull();
        item.Priority.Should().Be(12);
    }
    
    [Fact]
    public async Task InsertingSentinel_TriggersDatabaseDefault()
    {
        await using var writeDbContext = DemoDbContext.Create(output: output);
        writeDbContext.Set<ToDoItem>().Add(new ToDoItem("shopping")
        {
            Priority = -1
        });
        await writeDbContext.SaveChangesAsync();

        await using var readDbContext = DemoDbContext.Create();
        var item = await readDbContext.Set<ToDoItem>().FirstAsync();

        using var scope = new AssertionScope();
        item.Should().NotBeNull();
        item.Priority.Should().Be(ToDoItemConfiguration.PrioritySentinelValue);
    }
}