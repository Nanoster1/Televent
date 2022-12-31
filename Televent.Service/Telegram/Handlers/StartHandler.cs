using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[CommandHandler(CommandPrefix)]
public class StartHandler : IHandler
{
    public const string CQDataPrefix = "start";
    public const string CommandPrefix = "/start";

    private readonly ITelegramBotClient _bot;

    public StartHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task HandleAsync(Update update, CancellationToken token)
    {
        var chatId = update.Message!.Chat.Id;
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Правила игры", RulesHandler.CQDataPrefix),
                InlineKeyboardButton.WithCallbackData("Регистрация", RegistrationHandler.CQDataPrefix)
            }
        });

        await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: "Приветственное сообщение",
            replyMarkup: keyboard,
            cancellationToken: token);
    }
}