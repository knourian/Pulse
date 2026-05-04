using Microsoft.EntityFrameworkCore;

using Pulse.Server.Agents.Entities;
using Pulse.Server.Extensions;

namespace Pulse.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddCustomMappings();
    }

    public DbSet<Agent> Agents { get; set; }
}
