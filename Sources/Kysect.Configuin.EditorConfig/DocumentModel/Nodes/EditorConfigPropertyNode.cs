using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigPropertyNode(EditorConfigStringNode Key, EditorConfigStringNode Value, ImmutableList<string> LeadingTrivia, string? TrailingTrivia) : IEditorConfigNode
{
    public EditorConfigPropertyNode(EditorConfigStringNode key, EditorConfigStringNode value) : this(key, value, ImmutableList<string>.Empty, null)
    {
    }

    public EditorConfigPropertyNode(string key, string value) : this(EditorConfigStringNode.Create(key), EditorConfigStringNode.Create(value))
    {
    }

    public string ToFullString()
    {
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        string line = $"{Key.ToFullString()}={Value.ToFullString()}";
        if (TrailingTrivia is not null)
            line += $"{line} {TrailingTrivia}";

        stringBuilder.Append(line);
        return stringBuilder.ToString();
    }
}