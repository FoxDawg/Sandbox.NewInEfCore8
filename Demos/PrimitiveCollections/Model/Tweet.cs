using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demos.PrimitiveCollections.Model;

public class Tweet
{
    private Tweet()
    {
    }
    public int Id { get; }
    public string Name { get; }
    public List<string> Tags { get; }

    public Tweet(string name, IEnumerable<string> tags)
    {
        Name = name;
        Tags = tags.ToList();
    }
}

public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
{
    public void Configure(EntityTypeBuilder<Tweet> builder)
    {
        builder.ToTable("Tweets");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Tags);
    }
}
