using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Televent.Core.Events.Models;
using Televent.Core.Users.Models;

namespace Televent.Data;

public class TeleventContext : DbContext
{
    public const string ConnectionStringName = "Televent";

    public TeleventContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeleventContext).Assembly);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
}