using Televent.Core.Users.Models;
using Televent.Service.Telegram.Attributes.Shared;

namespace Televent.Service.Telegram.Attributes;

public class UserStateHandlerAttribute : HandlerBaseAttribute<string?>
{
    public string State { get; }

    public UserStateHandlerAttribute(string state)
    {
        State = state;
    }

    public override bool IsValid(string? value)
    {
        if (value == null) return false;
        return State == value;
    }
}