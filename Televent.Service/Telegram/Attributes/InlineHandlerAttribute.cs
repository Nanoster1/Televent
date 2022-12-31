using System.Text.RegularExpressions;
using Televent.Service.Telegram.Attributes.Shared;
using Televent.Service.Telegram.Constants;

namespace Televent.Service.Telegram.Attributes;

public class InlineHandlerAttribute : HandlerBaseAttribute<string>
{
    public string Prefix { get; }
    private Regex PrefixRegex { get; }

    public InlineHandlerAttribute(string data)
    {
        Prefix = data;
        PrefixRegex = new Regex($@"^{Prefix}({CQDefaults.PrefixDelimiter}.*|\s*)", RegexOptions.Compiled);
    }

    public override bool IsValid(string? value)
    {
        return PrefixRegex.IsMatch(value ?? string.Empty);
    }
}