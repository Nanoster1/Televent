namespace Televent.Core.Games.Models;

public class Game
{
    public Guid Id { get; set; }
    public required DateTimeOffset StartTime { get; set; }
    public required int PlayersCount { get; set; }
    public bool IsFinished { get; set; }
}