using Televent.Service.Telegram.Attributes.Shared;

namespace Televent.Service.Telegram.Attributes;

public class StaticCommandHandlerAttribute : HandlerBaseAttribute<string>
{
    public string Command { get; }

    public StaticCommandHandlerAttribute(string value)
    {
        Command = value;
    }

    public override bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return value.Equals(Command, StringComparison.InvariantCultureIgnoreCase);
    }
}