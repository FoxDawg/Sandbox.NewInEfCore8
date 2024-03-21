using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Demos;

public class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }

    public static DemoDbContext Create([CallerMemberName] string testName = "unknown", ITestOutputHelper? output = default)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        optionsBuilder.UseSqlite($"datasource=file:{testName}?mode=memory&cache=shared");
        optionsBuilder.LogTo((id, level) => id == RelationalEventId.CommandExecuted && level == LogLevel.Information, data => LogEvent(data, output));
        var dbContext = new DemoDbContext(optionsBuilder.Options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    private static void LogEvent(EventData data, ITestOutputHelper? output)
    {
        switch (data)
        {
            case CommandExecutedEventData commandExecuted:
                output?.WriteLine(commandExecuted.Command.CommandText);
                break;
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}