using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pulse.Server.Agents.Entities;

namespace Pulse.Server.Data.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("Agents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Hostname)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.MachineId)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasIndex(x => x.MachineId)
            .IsUnique();

        builder.Property(x => x.ApiKey)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.LastSeenUtc)
            .IsRequired();

        builder.HasIndex(x => x.ApiKey)
            .IsUnique();
    }
}
