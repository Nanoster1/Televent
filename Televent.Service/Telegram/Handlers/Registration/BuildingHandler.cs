using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.Building)]
public class BuildingHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public BuildingHandler(IUserManager userManager, ITelegramBotClient bot)
    {
        _userManager = userManager;
        _bot = bot;
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
        user.Building = int.Parse(message);
        user.State = RegistrationStates.Room;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "В какой комнате ты живёшь?",
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (!int.TryParse(message, out var room))
            return (false, "Корпус должен быть числом");

        return (true, string.Empty);
    }
}