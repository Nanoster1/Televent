using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Core.Common.Models;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Registration;

[UserStateHandler(RegistrationStates.Squad)]
public class SquadHandler : IHandler
{
    private readonly IUserManager _userManager;
    private readonly ITelegramBotClient _bot;
    private readonly PreloadedData _preloadedData;

    public SquadHandler(ITelegramBotClient bot, IUserManager userManager, IOptions<PreloadedData> preloadedData)
    {
        _bot = bot;
        _userManager = userManager;
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

        var keyboard = new ReplyKeyboardMarkup(Array.Empty<KeyboardButton>())
        {
            Keyboard = _preloadedData.Buildings.Select(b => new[] { new KeyboardButton(b) }).ToArray(),
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        var user = await _userManager.GetByIdAsync(update.Message!.From!.Id) ?? throw new NullReferenceException();
        user.Squad = int.Parse(message);
        user.State = RegistrationStates.Building;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "В каком корпусе ты живёшь?",
            replyMarkup: keyboard,
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        if (!int.TryParse(message, out var squad))
            return (false, "Отряд должен быть числом");

        return (true, string.Empty);
    }
}