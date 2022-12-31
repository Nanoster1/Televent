namespace Televent.Service.Telegram.Settings;

public class TelegramBotSettings
{
    public const string SectionName = "Telegram";
    public required string Token { get; init; }
}