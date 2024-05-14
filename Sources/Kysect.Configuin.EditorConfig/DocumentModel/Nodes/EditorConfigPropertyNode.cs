using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigPropertyNode(string Key, string Value, ImmutableList<string> LeadingTrivia, string? TrailingTrivia) : IEditorConfigNode
{
    public EditorConfigPropertyNode(string key, string value) : this(key, value, ImmutableList<string>.Empty, null)
    {
    }
}