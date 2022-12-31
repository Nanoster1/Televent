namespace Televent.Service.Telegram.Attributes.Shared;

public abstract class HandlerBaseAttribute<T> : Attribute
{
    public abstract bool IsValid(T? value);
}