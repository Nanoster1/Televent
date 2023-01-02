namespace Televent.Core.Events.Models;

public class Event
{
    public int Id { get; set; }
    public required string EventName { get; set; }
    public required string EventDescription { get; set; }
    public DateTimeOffset? ExecutionTime { get; set; }
    public bool IsExecuted { get; set; }
    public string? Message { get; set; }
    public string? Image { get; set; }
}