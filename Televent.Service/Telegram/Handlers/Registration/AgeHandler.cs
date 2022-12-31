using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.Age)]
public class AgeHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public AgeHandler(ITelegramBotClient bot, IUserManager userManager)
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
        user.Age = int.Parse(message);
        user.State = RegistrationStates.Squad;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Из какого ты отряда?",
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (!int.TryParse(message, out var age))
            return (false, "Возраст должен быть числом");

        if (age is < 5 or > 18)
            return (false, "Возраст должен быть от 5 до 18");

        return (true, string.Empty);
    }
}