using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pulse.Server.CheckRuns.Entities;

namespace Pulse.Server.Data.Configurations;

public class CheckResultConfiguration : IEntityTypeConfiguration<CheckResult>
{
    public void Configure(EntityTypeBuilder<CheckResult> builder)
    {
        builder.ToTable("CheckResult");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CheckId)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.AgentId)
            .HasMaxLength(32)
            .IsRequired();

        // Timestamp
        builder.Property(x => x.TimestampUtc)
            .IsRequired();

        // Result
        builder.Property(x => x.IsSuccess)
            .IsRequired();

        builder.Property(x => x.ResponseTimeMs)
            .IsRequired();

        builder.Property(x => x.Error)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.CheckId, x.TimestampUtc });

        builder.HasIndex(x => new { x.AgentId, x.TimestampUtc });

        builder.HasIndex(x => x.IsSuccess);
    }
}
