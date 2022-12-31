using Telegram.Bot.Types;

namespace Televent.Service.Telegram.Handlers.Interfaces;

public interface IHandler
{
    Task HandleAsync(Update update, CancellationToken token);
}