using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Game;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[CommandHandler(CommandPrefix, SecondCommandPrefix)]
public class StartHandler : IHandler
{
    public const string CommandPrefix = "/start";
    public const string SecondCommandPrefix = "/menu";

    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public StartHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var chatId = update.Message!.Chat.Id;
        var user = await _userManager.GetByIdAsync(update.Message.From!.Id) ?? throw new NullReferenceException();
        var welcomeText = update.Message.Text switch
        {
            SecondCommandPrefix => "Добро пожаловать в #ЛагерьЧе!",
            CommandPrefix => """
                Привет, друг!

                Веришь ли ты в Деда Мороза? Получил ты свой новогодний подарок 31 декабря?

                Каждому приятно получать подарки под елкой и в новогоднюю ночь, а потом смотреть салюты и радоваться, какой же классный подарок мне сделали на этот Новый Год.

                А что... Если и ты можешь стать для кого-то Дедом Морозом? Подарить человеку маленькое зимнее чудо на зимней смене в #ЛагерьЧе.

                Мы предлагаем тебе сыграть в известную новогоднюю игру – Тайный Дед Мороз ✨
                У тебя есть шанс подарить одному человеку на смене зимнее чудо и самому получить приятный подарок.

                Прими участие в игре вместе со всем лагерем! 250 человек объединятся, чтобы сделать эту смену по-настоящему праздничной и волшебной.
                """,
            _ => throw new ArgumentException()
        };

        if (user.Role == UserRole.Player)
        {

            var buttons = new List<InlineKeyboardButton>();
            if (!user.IsRegistered) buttons.Add(InlineKeyboardButton.WithCallbackData("Регистрация", RegistrationHandler.CQDataPrefix));
            buttons.Add(InlineKeyboardButton.WithCallbackData("Правила игры", RulesHandler.CQDataPrefix));
            var keyboard = new InlineKeyboardMarkup(buttons);

            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: welcomeText,
                replyMarkup: keyboard,
                cancellationToken: token);
        }
        else
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData("Начать игру", StartGameHandler.CQDataPrefix) },
                new [] { InlineKeyboardButton.WithCallbackData("Завершить регистрацию", FinishRegistrationHandler.CQDataPrefix) },
                new [] { InlineKeyboardButton.WithCallbackData("Правила игры", RulesHandler.CQDataPrefix) }
            });

            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: welcomeText,
                replyMarkup: keyboard,
                cancellationToken: token);
        }
    }
}