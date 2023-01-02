using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Televent.Core.Common.Models;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Services;

namespace Televent.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<IUserManager, UserManager>();
        services.Configure<PreloadedData>(configuration);
        return services;
    }
}