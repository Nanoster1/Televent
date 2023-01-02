using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.Finish)]
public class RegistrationFinishHandler : IHandler
{
    private readonly IUserManager _userManager;
    private readonly ITelegramBotClient _bot;

    public RegistrationFinishHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var message = update.Message?.Text;
        if (message == null) return;

        (var isValid, var errorMessage) = ValidateMessage(message);
        if (!isValid)
        {
            await _bot.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: errorMessage,
                cancellationToken: token);
            return;
        }

        var user = await _userManager.GetByIdAsync(update.Message!.From!.Id) ?? throw new NullReferenceException();

        if (message == "Да")
        {
            user.IsRegistered = true;
            user.State = Core.Users.Models.User.DefaultState;
            await _userManager.UpdateAsync(user);
            await _bot.SendTextMessageAsync(
           chatId: update.Message.Chat.Id,
           text: "Всё, теперь ты зарегистрирован!",
           replyMarkup: new ReplyKeyboardRemove(),
           cancellationToken: token);
        }
        else if (message == "Нет")
        {
            await _userManager.DeleteAsync(user);
            await _bot.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Жаль, что ты не хочешь зарегистрироваться. Напиши /start, если передумаешь.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: token);
        }
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (message == "Да")
        {
            return (true, string.Empty);
        }
        else if (message == "Нет")
        {
            return (true, string.Empty);
        }
        else
        {
            return (false, "Неверный ответ");
        }
    }
}