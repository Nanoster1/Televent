using Telegram.Bot;
using Televent.Core.Common.Interfaces;
using Televent.Core.Events.Interfaces;
using Televent.Service.Telegram.Interfaces;

namespace Televent.Service.Telegram.Workers;

public class EventWorker : BackgroundService
{
    private readonly IHandlerService _handlerService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<EventWorker> _logger;

    public EventWorker(
        IHandlerService handlerService,
        IServiceScopeFactory scopeFactory,
        ITelegramBotClient botClient,
        ILogger<EventWorker> logger)
    {
        _handlerService = handlerService;
        _serviceScopeFactory = scopeFactory;
        _botClient = botClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EventWorker is running");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    var eventRepository = serviceScope.ServiceProvider.GetRequiredService<IEventRepository>();
                    var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var events = await eventRepository.ListAllNotExecutedAsync();
                    foreach (var @event in events)
                    {
                        if (@event.ExecutionTime is not null && !@event.IsExecuted && @event.ExecutionTime <= DateTimeOffset.Now)
                        {
                            await _handlerService.ExecuteEvent(@event.EventName, serviceScope, @event, stoppingToken);
                            _logger.LogInformation($"Event {@event.EventName} executed");
                            @event.IsExecuted = true;
                            await eventRepository.UpdateAsync(@event);
                            await unitOfWork.SaveAsync();
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EventWorker error");
            }
        }
    }
}