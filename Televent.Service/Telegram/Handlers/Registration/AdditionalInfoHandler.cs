using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.AdditionalInfo)]
public class AdditionalInfoHandler : IHandler
{
    private readonly IUserManager _userManager;
    private readonly ITelegramBotClient _bot;

    public AdditionalInfoHandler(ITelegramBotClient bot, IUserManager userManager)
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
        user.AdditionalInfo = message;
        user.IsRegistered = true;
        user.State = Core.Users.Models.User.DefaultState;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Всё, теперь ты зарегистрирован!",
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        return (true, string.Empty);
    }
}