using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Televent.Core.Common.Interfaces;
using Televent.Core.Events.Interfaces;
using Televent.Core.Games.Interfaces;
using Televent.Core.Users.Interfaces;
using Televent.Data.Repositories;
using Televent.Data.Services;

namespace Televent.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeleventContext>(options =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            options.UseNpgsql(configuration.GetConnectionString(TeleventContext.ConnectionStringName))
                .UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        return services;
    }
}