using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.SentinelValues.Model;

public class ToDoItem
{
    public int Id { get; }
    public string Name { get; }
    public int Priority { get; init; }

    private ToDoItem()
    {
    }
    
    public ToDoItem(string name)
    {
        Name = name;
    }
}

public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public const int PrioritySentinelValue = 100;
    
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable("ToDoItems");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Priority).HasSentinel(-1).HasDefaultValueSql(PrioritySentinelValue.ToString());
    }
}