using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigStringNode(string Value, string LeadingTrivia, string TrailingTrivia)
{
    public static DotnetConfigStringNode Create(string value)
    {
        value.ThrowIfNull();

        string leadingTriviaLength = value.Substring(0, value.Length - value.TrimStart().Length);
        string trailingTriviaLength = value.Substring(value.TrimEnd().Length, value.Length - value.TrimEnd().Length);

        return new DotnetConfigStringNode(
            value.Trim(),
            leadingTriviaLength,
            trailingTriviaLength);
    }

    public string ToFullString()
    {
        return $"{LeadingTrivia}{Value}{TrailingTrivia}";
    }
}