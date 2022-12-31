using Televent.Service.Telegram.Attributes.Shared;

namespace Televent.Service.Telegram.Attributes;

public class CommandHandlerAttribute : HandlerBaseAttribute<string>
{
    public string Command { get; }

    public CommandHandlerAttribute(string command)
    {
        Command = command;
    }

    public override bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return string.Equals(Command, value, StringComparison.OrdinalIgnoreCase);
    }
}