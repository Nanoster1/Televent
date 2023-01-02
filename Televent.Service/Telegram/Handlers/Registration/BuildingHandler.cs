using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Common.Models;
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
    private readonly PreloadedData _preloadedData;

    public BuildingHandler(IUserManager userManager, ITelegramBotClient bot, IOptions<PreloadedData> preloadedData)
    {
        _userManager = userManager;
        _bot = bot;
        _preloadedData = preloadedData.Value;
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
        user.Building = message;
        user.State = RegistrationStates.Room;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "В какой комнате ты живёшь?",
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (_preloadedData.Buildings.All(b => b != message))
            return (false, "Такого корпуса нет");

        return (true, string.Empty);
    }
}