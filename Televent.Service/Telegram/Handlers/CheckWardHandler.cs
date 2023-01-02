using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[StaticCommandHandler("/my_ward")]
public class CheckWardHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;

    public CheckWardHandler(ITelegramBotClient bot, IUserManager userManager)
    {
        _bot = bot;
        _userManager = userManager;
    }

    public async Task HandleAsync(Update update, CancellationToken token)
    {
        var chatId = update.Message!.Chat.Id;
        var user = await _userManager.GetByIdAsync(update.Message.From!.Id) ?? throw new NullReferenceException();
        var ward = user.WardId is null ? null : await _userManager.GetByIdAsync(user.WardId.Value);
        var text = ward is null ? "Вы не имеете подопечного" : $"Ваш подопечный: {ward.NameAndSurname}";

        await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            cancellationToken: token);
    }
}