using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Games.Interfaces;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[InlineHandler(CQDataPrefix)]
public class RegistrationHandler : IHandler
{
    public const string CQDataPrefix = "registration";

    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;
    private readonly IGameRepository _gameRepository;

    public RegistrationHandler(ITelegramBotClient bot, IUserManager userManager, IGameRepository gameRepository)
    {
        _bot = bot;
        _userManager = userManager;
        _gameRepository = gameRepository;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var chatId = update.CallbackQuery!.Message!.Chat.Id;
        var messageId = update.CallbackQuery.Message.MessageId;
        var text = """
        Мы рады, что ты решился принять участие в игре “Тайный Дед Мороз” ✨

        Давай добавим тебя в список Дедов Морозов и узнаем, какой подарок ты хочешь получить. Для этого тебе нужно ответить на несколько вопросов
        """;
        var lastGame = await _gameRepository.GetLastGameAsync();

        if (lastGame is not null && !lastGame.IsFinished)
        {
            text = "Игра уже началась, регистрация невозможна";
            await _bot.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: text,
                cancellationToken: token
            );
            return;
        }

        var user = await _userManager.GetByIdAsync(update.CallbackQuery.From.Id) ?? throw new NullReferenceException();
        user.State = RegistrationStates.NameAndSurname;
        await _userManager.UpdateAsync(user);

        var tgUser = update.CallbackQuery.From;
        ReplyMarkupBase? keyboard = null;
        if (tgUser.FirstName is not null && tgUser.LastName is not null)
        {
            var btnText = $"{update.CallbackQuery.From.FirstName} {update.CallbackQuery.From.LastName}";
            keyboard = new ReplyKeyboardMarkup(new KeyboardButton(btnText)) { ResizeKeyboard = true };
        }
        await _bot.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            cancellationToken: token);

        await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: "Напиши свою фамилию и имя",
            replyMarkup: keyboard,
            cancellationToken: token);

        await _bot.AnswerCallbackQueryAsync(
            callbackQueryId: update.CallbackQuery.Id,
            cancellationToken: token);
    }
}