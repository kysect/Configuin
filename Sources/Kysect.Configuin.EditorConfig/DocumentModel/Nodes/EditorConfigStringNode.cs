using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigStringNode(string Value, string LeadingTrivia, string TrailingTrivia) : IEditorConfigNode
{
    public static EditorConfigStringNode Create(string value)
    {
        value.ThrowIfNull();

        string leadingTriviaLength = value.Substring(0, value.Length - value.TrimStart().Length);
        string trailingTriviaLength = value.Substring(value.TrimEnd().Length, value.Length - value.TrimEnd().Length);

        return new EditorConfigStringNode(
            value.Trim(),
            leadingTriviaLength,
            trailingTriviaLength);
    }

    public string ToFullString()
    {
        return $"{LeadingTrivia}{Value}{TrailingTrivia}";
    }
}