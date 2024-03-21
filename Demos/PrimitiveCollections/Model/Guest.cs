using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.PrimitiveCollections.Model;

public class Guest
{
    private Guest()
    {
    }
    public int Id { get; }
    public string Name { get; }
    public List<BreakfastOptions> BreakfastOptions { get; }

    public Guest(string name, IEnumerable<BreakfastOptions> breakfastOptions)
    {
        Name = name;
        BreakfastOptions = breakfastOptions.ToList();
    }
}

public enum BreakfastOptions
{
    Toast,
    Tea,
    Juice
}

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        builder.Property(o => o.BreakfastOptions);
    }
}