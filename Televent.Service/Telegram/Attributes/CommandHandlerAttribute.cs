using Televent.Service.Telegram.Attributes.Shared;

namespace Televent.Service.Telegram.Attributes;

public class CommandHandlerAttribute : HandlerBaseAttribute<string>
{
    public string[] Commands { get; }

    public CommandHandlerAttribute(params string[] commands)
    {
        Commands = commands;
    }

    public override bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return Commands.Any(command => command.Equals(value, StringComparison.OrdinalIgnoreCase));
    }
}