using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pulse.Server.Checks.Entities;

namespace Pulse.Server.Data.Configurations;

public class CheckConfiguration : IEntityTypeConfiguration<Check>
{
    public void Configure(EntityTypeBuilder<Check> builder)
    {
        builder.ToTable("Checks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Target)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Signature)
            .HasMaxLength(600)
            .IsRequired();

        builder.Property(x => x.IntervalSeconds)
            .IsRequired();

        builder.Property(x => x.TimeoutMs)
            .IsRequired();

        builder.Property(x => x.Enabled)
            .IsRequired();

        builder.Property(x => x.CreatedUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedUtc);

        builder.HasIndex(x => x.Enabled);

        builder.HasIndex(x => x.Signature)
            .IsUnique();

        builder.HasIndex(x => x.Type);
    }
}