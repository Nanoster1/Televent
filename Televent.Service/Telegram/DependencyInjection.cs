using Microsoft.Extensions.Options;
using Telegram.Bot;
using Televent.Service.Telegram.Handlers.Interfaces;
using Televent.Service.Telegram.Interfaces;
using Televent.Service.Telegram.Services;
using Televent.Service.Telegram.Settings;

namespace Televent.Service.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegram(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramBotSettings>(configuration.GetSection(TelegramBotSettings.SectionName));
        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
            return new TelegramBotClient(settings.Token);
        });
        services.AddHostedService<TeleBot>();
        services.AddHostedService<TestService>();
        services.AddSingleton<IHandlerService, HandlerService>();
        services.AddHandlers();
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(TeleBot).Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IHandler)));

        foreach (var type in types) services.AddScoped(type);

        return services;
    }
}