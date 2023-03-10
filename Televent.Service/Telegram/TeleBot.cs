using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Services;
using Televent.Service.Telegram.Interfaces;
using User = Televent.Core.Users.Models.User;

namespace Televent.Service.Telegram;

public class TeleBot : BackgroundService
{
    private readonly ILogger<TeleBot> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHandlerService _handlerService;


    public TeleBot(
        ILogger<TeleBot> logger,
        ITelegramBotClient telegramBotClient,
        IServiceScopeFactory scopeFactory,
        IHandlerService handlerService)
    {
        _logger = logger;
        _telegramBotClient = telegramBotClient;
        _scopeFactory = scopeFactory;
        _handlerService = handlerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var commands = new List<BotCommand>
                {
                    new() { Command = "/start", Description = "Стартовое меню" },
                    new() { Command = "/menu", Description = "Мини стартовое меню" },
                    new() { Command = "/rules", Description = "Правила" },
                    new() { Command = "/my_ward", Description = "Мой подопечный" },
                    new() { Command = "/message", Description = "Создать сообщение" }
                };
                await _telegramBotClient.SetMyCommandsAsync(commands);
                _logger.LogInformation($"Bot running at: {DateTimeOffset.Now}");

                var receiverOptions = new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                var me = await _telegramBotClient.GetMeAsync(stoppingToken);
                var botName = me.Username ?? throw new InvalidOperationException("Bot username is null");
                var selfLink = new Uri($"http://t.me/{botName}");

                _logger.LogInformation($"Bot is available at: {selfLink}");

                await _telegramBotClient.ReceiveAsync(
                    updateHandler: HandleUpdateAsync,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: stoppingToken
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while receiving updates");
            }
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Received update: {update.Id}");
            var scope = _scopeFactory.CreateScope();
            var user = await GetUser(update, scope.ServiceProvider.GetRequiredService<IUserManager>(), cancellationToken);
            if (user is null) return;
            var handler = _handlerService.ChooseHandler(update, scope, user);
            if (handler is null) return;
            await handler.HandleAsync(update, null, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while handling update");
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogWarning(ErrorMessage);
        return Task.CompletedTask;
    }

    private async Task<User?> GetUser(Update update, IUserManager userManager, CancellationToken cancellationToken)
    {
        var tgUser = update.MyChatMember?.From ?? update.Message?.From ?? update.CallbackQuery?.From;

        if (tgUser is null) return null;

        var user = await userManager.GetByIdAsync(tgUser.Id);
        if (user is not null) return user;

        user = User.CreateDefault(
            tgUser.Id, update.Message?.Chat.Id ??
            update.CallbackQuery?.Message?.Chat.Id ??
            update.MyChatMember!.Chat.Id);
        await userManager.AddAsync(user);
        return user;
    }
}
