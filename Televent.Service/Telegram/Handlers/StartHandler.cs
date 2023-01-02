using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[CommandHandler(CommandPrefix)]
public class StartHandler : IHandler
{
    public const string CQDataPrefix = "start";
    public const string CommandPrefix = "/start";

    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public StartHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, CancellationToken token)
    {
        var chatId = update.Message!.Chat.Id;
        var user = await _userManager.GetByIdAsync(update.Message.From!.Id) ?? throw new NullReferenceException();

        if (user.Role is UserRole.Player)
        {

            var buttons = new List<InlineKeyboardButton>();
            if (!user.IsRegistered) buttons.Add(InlineKeyboardButton.WithCallbackData("Регистрация", RegistrationHandler.CQDataPrefix));
            buttons.Add(InlineKeyboardButton.WithCallbackData("Правила игры", RulesHandler.CQDataPrefix));
            var keyboard = new InlineKeyboardMarkup(buttons);

            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: "Приветственное сообщение",
                replyMarkup: keyboard,
                cancellationToken: token);
        }
        else
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData("Начать игру", string.Empty) },
                new [] { InlineKeyboardButton.WithCallbackData("Пересоздать", string.Empty) },
                new [] { InlineKeyboardButton.WithCallbackData("Правила игры", RulesHandler.CQDataPrefix) }
            });

            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: "Приветственное сообщение",
                replyMarkup: keyboard,
                cancellationToken: token);
        }
    }
}