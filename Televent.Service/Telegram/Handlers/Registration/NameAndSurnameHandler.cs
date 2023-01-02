using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.NameAndSurname)]
public class NameAndSurnameHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public NameAndSurnameHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, CancellationToken token)
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
        user.NameAndSurname = message;
        user.State = RegistrationStates.Age;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Сколько тебе лет?",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return (false, "Имя не может быть пустым");

        var nameAndSurname = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (nameAndSurname.Length != 2)
            return (false, "Неверный формат имени");

        return (true, string.Empty);
    }
}