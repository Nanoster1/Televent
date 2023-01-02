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
public class StartGameHandler : IHandler
{
    public const string CQDataPrefix = "start_game";

    private readonly ITelegramBotClient _botClient;
    private readonly IUserManager _userService;
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<StartGameHandler> _logger;

    public StartGameHandler(
        ITelegramBotClient botClient,
        IUserManager userService,
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork,
        ILogger<StartGameHandler> logger)
    {
        _botClient = botClient;
        _userService = userService;
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var users = await _userService.ListAllAsync().Where(u => u.IsRegistered).ToArrayAsync(token);

        for (int i = users.Length - 1; i >= 1; i--)
        {
            int j = Random.Shared.Next(i + 1);
            var temp = users[j];
            users[j] = users[i];
            users[i] = temp;
        }

        var lastGame = await _gameRepository.GetLastGameAsync();
        if (lastGame is null || lastGame.IsFinished)
        {
            await _botClient.AnswerCallbackQueryAsync(
                update.CallbackQuery!.Id,
                "Регистрация ещё не закончена",
                cancellationToken: token);
            return;
        }

        for (int i = 0; i < users.Length; i++)
        {
            if (i + 1 == users.Length) users[i].WardId = users[0].Id;
            else users[i].WardId = users[i + 1].Id;
        }

        if (lastGame is not null) await _gameRepository.UpdateAsync(lastGame);
        await _userService.UpdateAsync(users);

        var message = $"Игра началась! Вот список участников:{Environment.NewLine}";
        for (int i = 0; i < users.Length; i++)
        {
            var username = users[i].NameAndSurname;
            var ward = users.First(u => u.Id == users[i].WardId).NameAndSurname;
            message += $"{i + 1}. {username} -> {ward}\n";
        }

        await _botClient.EditMessageTextAsync(
            update.CallbackQuery!.Message!.Chat.Id,
            update.CallbackQuery.Message.MessageId,
            message,
            cancellationToken: token);

        foreach (var user in users)
        {
            if (user.ChatId is null) continue;
            var ward = users.First(u => u.Id == user.WardId);
            var text = $"""
            Твой подопечный: {ward.NameAndSurname}
            Предпочтения: {ward.AdditionalInfo}
            Корпус: {ward.Building}
            Комната: {ward.Room}
            Возраст: {ward.Age}
            """;
            try
            {
                await _botClient.SendTextMessageAsync(
                    user.ChatId,
                    text,
                    cancellationToken: token);
            }
            catch (ApiRequestException e)
            {
                _logger.LogError(e.Message);
            }
        }

        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, cancellationToken: token);
    }
}