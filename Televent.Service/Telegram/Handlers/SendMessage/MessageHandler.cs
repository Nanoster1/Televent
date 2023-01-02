using Telegram.Bot;
using Telegram.Bot.Types;
using Televent.Core.Common.Interfaces;
using Televent.Core.Events.Interfaces;
using Televent.Core.Events.Models;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.SendMessage;

[UserStateHandler(SendMessageState.Message)]
public class MessageHandler : IHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserManager _userManager;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MessageHandler(ITelegramBotClient bot, IUserManager userManager, IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _bot = bot;
        _userManager = userManager;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var photo = update.Message?.Photo?[0];
        var message = photo is null ? update.Message?.Text : update.Message?.Caption;
        if (message == null) return;

        DateTimeOffset? time = null;
        var parsedMessage = string.Empty;
        try
        {
            if (message.Contains(Environment.NewLine))
                parsedMessage = message.Substring(0, message.IndexOf(Environment.NewLine));
            else
                parsedMessage = message;
            time = DateTimeOffset.Parse(parsedMessage);
        }
        catch (Exception)
        {
            await _bot.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "Неверный формат времени",
                cancellationToken: token);
            return;
        }

        message = message.Replace(parsedMessage, string.Empty);
        if (string.IsNullOrWhiteSpace(message) && photo is null)
        {
            await _bot.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "Сообщение не может быть пустым",
                cancellationToken: token);
            return;
        }

        var @event = new Event
        {
            EventName = "message",
            Message = message,
            EventDescription = "created by bot",
            ExecutionTime = time,
            Image = photo?.FileId
        };

        await _eventRepository.InsertAsync(@event);
        await _unitOfWork.SaveAsync();

        var user = await _userManager.GetByIdAsync(update.Message!.From!.Id) ?? throw new NullReferenceException();
        user.State = Televent.Core.Users.Models.User.DefaultState;
        await _userManager.UpdateAsync(user);

        await _bot.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text: "Сообщение создано",
            cancellationToken: token);
    }
}