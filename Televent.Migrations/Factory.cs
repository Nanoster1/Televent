using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Televent.Data;

namespace Televent.Migrations;

public class Factory : IDesignTimeDbContextFactory<TeleventContext>
{
    public TeleventContext CreateDbContext(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        var configuration = configurationBuilder.Build();
        var connectionString = configuration.GetConnectionString(TeleventContext.ConnectionStringName);

        var optionsBuilder = new DbContextOptionsBuilder<TeleventContext>()
            .UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(Factory).Assembly.GetName().Name))
            .UseSnakeCaseNamingConvention();

        return new TeleventContext(optionsBuilder.Options);
    }
}
