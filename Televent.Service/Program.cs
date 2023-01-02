using Televent.Core;
using Televent.Data;
using Televent.Service.Configurations.AppConfig;
using Televent.Service.Telegram;

var host = Host.CreateDefaultBuilder(args);
host.ConfigureAppConfiguration(AppConfigBootstrap.SetUpAppConfiguration);

host.ConfigureServices((context, services) =>
{
    services.AddCore();
    services.AddData(context.Configuration);
    services.AddTelegram(context.Configuration);
});

var app = host.Build();
app.Run();