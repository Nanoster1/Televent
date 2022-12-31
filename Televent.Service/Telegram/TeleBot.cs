using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Handlers.Interfaces;
using Televent.Service.Telegram.Interfaces;
using User = Televent.Core.Users.Models.User;

namespace Televent.Service.Telegram;

public class TeleBot : BackgroundService
{
    private readonly ILogger<TeleBot> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUserManager _userManager;
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
        _userManager = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserManager>();
        _handlerService = handlerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Received update: {update.Id}");
        var scope = _scopeFactory.CreateScope();
        var user = await GetUser(update, cancellationToken);
        var handler = _handlerService.ChooseHandler(update, scope, user);
        if (handler is null) return;
        await handler.HandleAsync(update, cancellationToken);
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

    private async Task<User> GetUser(Update update, CancellationToken cancellationToken)
    {
        var tgUser = update.Message?.From ??
            update.CallbackQuery?.From;

        if (tgUser is null) throw new InvalidOperationException("User is null");

        var user = await _userManager.GetByIdAsync(tgUser.Id);
        if (user is not null) return user;

        user = User.CreateDefault(tgUser.Id);
        await _userManager.AddAsync(user);
        return user;
    }
}
