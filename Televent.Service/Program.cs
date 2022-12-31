using Serilog;
using Televent.Core;
using Televent.Data;
using Televent.Service.Telegram;

var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
    services.AddCore();
    services.AddData(context.Configuration);
    services.AddTelegram(context.Configuration);
});

host.ConfigureLogging((context, logging) =>
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(context.Configuration)
        .CreateLogger();
    logging.AddSerilog(logger);
});

var app = host.Build();
app.Run();