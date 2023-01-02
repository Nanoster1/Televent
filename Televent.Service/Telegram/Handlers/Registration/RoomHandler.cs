using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.Room)]
public class RoomHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public RoomHandler(IUserManager userManager, ITelegramBotClient bot)
    {
        _userManager = userManager;
        _bot = bot;
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
        user.Room = int.Parse(message);
        user.State = RegistrationStates.AdditionalInfo;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Расскажи о своих любимых вещах? Цвет, животные, увлечения, что угодно 😀",
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (!int.TryParse(message, out var room))
            return (false, "Комната должна быть числом");

        return (true, string.Empty);
    }
}