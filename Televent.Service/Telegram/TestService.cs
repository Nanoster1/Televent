namespace Televent.Service.Telegram;

public class TestService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var isCanceled = false;
        Console.CancelKeyPress += (sender, args) =>
        {
            isCanceled = true;
            args.Cancel = true;
        };
        while (!isCanceled && !stoppingToken.IsCancellationRequested)
        {
            var message = Console.ReadLine();
            Console.WriteLine(message);
        };
        return Task.CompletedTask;
    }
}