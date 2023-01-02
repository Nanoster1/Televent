using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Televent.Core.Events.Models;
using Televent.Core.Users.Interfaces;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers.Events;

[EventHandler("message")]
public class MessageEventHandler : IHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserManager _userService;
    private readonly ILogger<MessageEventHandler> _logger;

    public MessageEventHandler(ITelegramBotClient botClient, IUserManager userService, ILogger<MessageEventHandler> logger)
    {
        _botClient = botClient;
        _userService = userService;
        _logger = logger;
    }

    public async Task HandleAsync(Update? update, object? extraData = null, CancellationToken token = default)
    {
        if (extraData is not Event @event) throw new ArgumentException("Extra data must be of type Event", nameof(extraData));
        var blockedUsers = new List<Televent.Core.Users.Models.User>();
        await foreach (var user in _userService.ListAllAsync())
        {
            try
            {
                if (user.ChatId is not null && @event.Message is not null)
                {
                    if (@event.Image is null)
                        await _botClient.SendTextMessageAsync(user.ChatId, @event.Message, cancellationToken: token);
                    else
                        await _botClient.SendPhotoAsync(user.ChatId, @event.Image, @event.Message, cancellationToken: token);
                }
            }
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 403) blockedUsers.Add(user);
                else _logger.LogError(e.Message);
            }
        }
        foreach (var user in blockedUsers)
        {
            await _userService.DeleteAsync(user);
        }
    }
}