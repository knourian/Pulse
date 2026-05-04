using Microsoft.EntityFrameworkCore;

namespace Pulse.Server.Extensions;

public static class DatabaseMapping
{
    /// <summary>
    ///     Adds all of the ASP.NET Core mappings at once.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddCustomMappings(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseMapping).Assembly);
    }
}
