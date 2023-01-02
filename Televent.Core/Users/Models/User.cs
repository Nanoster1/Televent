namespace Televent.Core.Users.Models;

public class User
{
    public const string DefaultState = "None";

    public static User CreateDefault(long id, long chatId) => new()
    {
        Id = id,
        ChatId = chatId,
        State = DefaultState
    };

    public required long Id { get; init; }
    public required string State { get; set; }
    public UserRole Role { get; set; } = UserRole.Player;
    public bool IsRegistered { get; set; }
    public long? WardId { get; set; }
    public string? NameAndSurname { get; set; }
    public int? Age { get; set; }
    public int? Squad { get; set; }
    public string? Building { get; set; }
    public int? Room { get; set; }
    public string? AdditionalInfo { get; set; }
    public long? ChatId { get; init; }
}