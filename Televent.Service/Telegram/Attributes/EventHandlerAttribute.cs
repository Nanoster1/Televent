using Televent.Service.Telegram.Attributes.Shared;

namespace Televent.Service.Telegram.Attributes;

public class EventHandlerAttribute : HandlerBaseAttribute<string>
{
    public string EventName { get; }

    public EventHandlerAttribute(string command)
    {
        EventName = command;
    }

    public override bool IsValid(string? value)
    {
        return EventName.Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}