using Telegram.Bot;
using Telegram.Bot.Types;
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

    public RegistrationHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, CancellationToken token)
    {
        var chatId = update.CallbackQuery!.Message!.Chat.Id;
        var messageId = update.CallbackQuery.Message.MessageId;
        var text = "Регистрация";

        var user = await _userManager.GetByIdAsync(update.CallbackQuery.From.Id) ?? throw new NullReferenceException();
        user.State = RegistrationStates.AllStates[0];

        await _bot.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            cancellationToken: token);

        await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: "Напиши свою фамилию и имя",
            cancellationToken: token);

        await _bot.AnswerCallbackQueryAsync(
            callbackQueryId: update.CallbackQuery.Id,
            cancellationToken: token);
    }
}