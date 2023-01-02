namespace Televent.Core.Common.Models;

public class CommandResult
{
    public required string Message { get; init; }
    public IEnumerable<string>? PossibleAnswers { get; init; }
    public IDictionary<string, string>? PossibleCommands { get; init; }
    public AnswerType AnswerType { get; init; }
}