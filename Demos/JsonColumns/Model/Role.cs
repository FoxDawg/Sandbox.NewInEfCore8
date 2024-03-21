using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.JsonColumns.Model;

public class Role
{
    public int Id { get; }
    public string Name { get; }

    private Role()
    {
    }

    public Role(string name, IEnumerable<CustomPolicy> policies)
    {
        Name = name;
        this.Policies = policies.ToList();
    }

    public IList<CustomPolicy> Policies { get; } = new List<CustomPolicy>();
}

public class CustomPolicy
{
    public string Key { get; }
    public bool Value { get; }

    private CustomPolicy()
    {}
    public CustomPolicy(string key, bool value)
    {
        Key = key;
        Value = value;
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(o => o.Name).IsRequired().HasMaxLength(25);
        builder.OwnsMany(o => o.Policies, b =>
        {
            b.Property(p => p.Key);
            b.Property(p => p.Value);
            b.ToJson();
            //b.WithOwner();
        });
    }
}