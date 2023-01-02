using Telegram.Bot.Types;
using Televent.Service.Telegram.Handlers.Interfaces;
using User = Televent.Core.Users.Models.User;

namespace Televent.Service.Telegram.Interfaces;

public interface IHandlerService
{
    IHandler? ChooseHandler(Update updateType, IServiceScope scope, User user);
    Task ExecuteEvent(string eventName, IServiceScope scope, object? extraData = null, CancellationToken cancellationToken = default);
}