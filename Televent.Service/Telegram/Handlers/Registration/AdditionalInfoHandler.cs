using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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
        user.AdditionalInfo = message;
        user.State = RegistrationStates.Finish;
        await _userManager.UpdateAsync(user);
        await _bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Точно хочешь зарегистрироваться?",
            replyMarkup: new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Да"),
                new KeyboardButton("Нет")
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            },
            cancellationToken: token);
    }

    private (bool, string) ValidateMessage(string message)
    {
        return (true, string.Empty);
    }
}