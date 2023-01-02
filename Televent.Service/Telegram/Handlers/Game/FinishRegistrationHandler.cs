using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Televent.Core.Common.Interfaces;
using Televent.Core.Games.Interfaces;
using Televent.Core.Users.Interfaces;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;
using GameModel = Televent.Core.Games.Models.Game;

namespace Televent.Service.Telegram.Handlers.Game;

[InlineHandler(CQDataPrefix)]
public class FinishRegistrationHandler : IHandler
{
    public const string CQDataPrefix = "finish_registration";

    private readonly ITelegramBotClient _botClient;
    private readonly IUserManager _userService;
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FinishRegistrationHandler> _logger;

    public FinishRegistrationHandler(
        ITelegramBotClient botClient,
        IUserManager userService,
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork,
        ILogger<FinishRegistrationHandler> logger)
    {
        _botClient = botClient;
        _userService = userService;
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var game = await _gameRepository.GetLastGameAsync();
        var users = await _userService.ListAllAsync().Where(u => u.IsRegistered).ToArrayAsync(token);
        if (game is not null) game.IsFinished = true;
        var newGame = new GameModel
        {
            PlayersCount = users.Count(),
            StartTime = DateTimeOffset.Now
        };
        await _gameRepository.InsertAsync(newGame);
        await _unitOfWork.SaveAsync();

        foreach (var user in users)
        {
            if (user.ChatId is null) continue;
            try
            {
                await _botClient.SendTextMessageAsync(
                    user.ChatId,
                    """
                Все Тайные Дед Морозы зарегистрированы, утром вы узнаете, кому вам предстоит дарить подарок.
                """,
                    cancellationToken: token);
            }
            catch (ApiRequestException e)
            {
                _logger.LogError(e.Message);
            }
        }

        await _botClient.AnswerCallbackQueryAsync(
            update.CallbackQuery!.Id,
            "Регистрация завершена",
            cancellationToken: token);
    }
}