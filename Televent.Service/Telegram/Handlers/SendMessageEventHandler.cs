using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[CommandHandler(CommandPrefix)]
public class SendMessageEventHandler : IHandler
{
    public const string CommandPrefix = "/message";

    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public SendMessageEventHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var message = update.Message?.Text;
        if (message == null) return;

        var user = await _userManager.GetByIdAsync(update.Message!.From!.Id) ?? throw new NullReferenceException();
        if (user.Role != UserRole.Creator)
        {
            await _bot.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "Вы не являетесь администратором",
                cancellationToken: token);
            return;
        }

        user.State = SendMessageState.Message;
        await _userManager.UpdateAsync(user);

        var chatId = update.Message!.Chat.Id;
        var text = """
        Введите сообщение, которое хотите отправить всем пользователям, 
        где первая строчка - это дата вида: yyyy-MM-ddTHH:mm:sszzzz
        Пример: 
        2021-08-31T10:10:10+03:00 - это 31 августа 2021 года в 10:10:10 по московскому времени
        """;
        await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            cancellationToken: token);
    }
}