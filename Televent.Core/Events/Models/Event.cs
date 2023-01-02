namespace Televent.Core.Events.Models;

public class Event
{
    public int Id { get; set; }
    public required string EventName { get; set; }
    public required string EventDescription { get; set; }
    public required DateTimeOffset ExecutionTime { get; set; }
    public bool IsExecuted { get; set; }
}