using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Handlers.Interfaces;
using Televent.Service.Telegram.Interfaces;
using User = Televent.Core.Users.Models.User;

namespace Televent.Service.Telegram.Services;

public class HandlerService : IHandlerService
{
    private readonly Dictionary<CommandHandlerAttribute, Type> _commandHandlers = new();
    private readonly Dictionary<InlineHandlerAttribute, Type> _inlineHandlers = new();
    private readonly Dictionary<UserStateHandlerAttribute, Type> _messageHandlers = new();
    private readonly Dictionary<StaticCommandHandlerAttribute, Type> _staticCommandHandlers = new();
    private readonly Dictionary<EventHandlerAttribute, Type> _eventHandlers = new();

    public HandlerService()
    {
        FindHandlers();
    }

    public async Task ExecuteEvent(string eventName, IServiceScope scope, object? extraData = null, CancellationToken cancellationToken = default)
    {
        foreach (var (attr, type) in _eventHandlers)
        {
            if (attr.IsValid(eventName))
            {
                var handler = (IHandler)scope.ServiceProvider.GetRequiredService(type);
                await handler.HandleAsync(null!, extraData, cancellationToken);
            }
        }
    }

    public IHandler? ChooseHandler(Update update, IServiceScope scope, User user)
    {
        IHandler? handler = null;

        if (update.Type == UpdateType.Message)
        {
            foreach (var (attr, type) in _staticCommandHandlers)
            {
                if (attr.IsValid(update.Message?.Text))
                {
                    return (IHandler)scope.ServiceProvider.GetRequiredService(type);
                }
            }
        }

        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
                foreach (var (attr, type) in _inlineHandlers)
                {
                    if (attr.IsValid(update.CallbackQuery?.Data))
                    {
                        handler = (IHandler)scope.ServiceProvider.GetRequiredService(type);
                    }
                }
                break;

            case UpdateType.Message when user.State == User.DefaultState:
                foreach (var (attr, type) in _commandHandlers)
                {
                    if (attr.IsValid(update.Message?.Text))
                    {
                        handler = (IHandler)scope.ServiceProvider.GetRequiredService(type);
                    }
                }
                break;

            case UpdateType.Message when user.State != User.DefaultState:
                foreach (var (attr, type) in _messageHandlers)
                {
                    if (attr.IsValid(user.State))
                    {
                        handler = (IHandler)scope.ServiceProvider.GetRequiredService(type);
                    }
                }
                break;
        }

        return handler;
    }

    private void FindHandlers()
    {
        var assembly = typeof(TeleBot).Assembly;
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var commandHandlerAttribute = type.GetCustomAttribute<CommandHandlerAttribute>();
            if (commandHandlerAttribute is not null)
            {
                _commandHandlers.Add(commandHandlerAttribute, type);
            }

            var inlineHandlerAttribute = type.GetCustomAttribute<InlineHandlerAttribute>();
            if (inlineHandlerAttribute is not null)
            {
                _inlineHandlers.Add(inlineHandlerAttribute, type);
            }

            var messageHandlerAttribute = type.GetCustomAttribute<UserStateHandlerAttribute>();
            if (messageHandlerAttribute is not null)
            {
                _messageHandlers.Add(messageHandlerAttribute, type);
            }

            var staticCommandHandlerAttribute = type.GetCustomAttribute<StaticCommandHandlerAttribute>();
            if (staticCommandHandlerAttribute is not null)
            {
                _staticCommandHandlers.Add(staticCommandHandlerAttribute, type);
            }

            var eventHandlerAttribute = type.GetCustomAttribute<EventHandlerAttribute>();
            if (eventHandlerAttribute is not null)
            {
                _eventHandlers.Add(eventHandlerAttribute, type);
            }
        }
    }
}