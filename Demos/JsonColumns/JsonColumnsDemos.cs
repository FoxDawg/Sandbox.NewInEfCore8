using System.Linq;
using System.Threading.Tasks;
using Demos.JsonColumns.Model;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Demos.JsonColumns;

public class JsonColumnsDemos
{
    private readonly ITestOutputHelper output;

    public JsonColumnsDemos(ITestOutputHelper output)
    {
        this.output = output;
    }
    [Fact]
    public async Task PropertiesAreMappedToJsonColumns()
    {
        await using var writeDbContext = DemoDbContext.Create(nameof(PropertiesAreMappedToJsonColumns), this.output);

        CustomPolicy[] policies =
        [
            new CustomPolicy("canOrderCoffee", true),
            new CustomPolicy("canPayCash", false),
        ];
        var role = new Role("MyCustomRole", policies);
        writeDbContext.Set<Role>().Add(role);
        await writeDbContext.SaveChangesAsync();
        
        await using var readDbContext = DemoDbContext.Create(nameof(PropertiesAreMappedToJsonColumns));
        var existingRole = readDbContext.Set<Role>().First();

        existingRole.Policies.Should().ContainSingle(p => p.Key.Contains("Coffee") && p.Value);
        existingRole.Policies.Should().ContainSingle(p => p.Key.Contains("Cash") && !p.Value);
    }
}