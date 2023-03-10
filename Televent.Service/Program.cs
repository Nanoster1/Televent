using Serilog;
using Televent.Core;
using Televent.Data;
using Televent.Service.Configurations.AppConfig;
using Televent.Service.Telegram;

var host = Host.CreateDefaultBuilder(args);
host.ConfigureAppConfiguration(AppConfigBootstrap.SetUpAppConfiguration);
host.UseSerilog((ctx, logger) =>
{
    logger.ReadFrom.Configuration(ctx.Configuration);
});

host.UseSystemd().ConfigureServices((context, services) =>
{
    services.AddCore(context.Configuration);
    services.AddData(context.Configuration);
    services.AddTelegram(context.Configuration);
});

var app = host.Build();
app.Run();