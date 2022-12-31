using Microsoft.Extensions.DependencyInjection;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Services;

namespace Televent.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IUserManager, UserManager>();
        return services;
    }
}