using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigDocumentSectionNode(string Value, ImmutableList<IEditorConfigNode> Children, ImmutableList<string> LeadingTrivia, string? TrailingTrivia) : IEditorConfigContainerNode
{
    public const string NodeIndicator = "###";

    public EditorConfigDocumentSectionNode(string value) : this(value, ImmutableList<IEditorConfigNode>.Empty, ImmutableList<string>.Empty, null)
    {
    }

    IEditorConfigContainerNode IEditorConfigContainerNode.AddChild(IEditorConfigNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public EditorConfigDocumentSectionNode AddChild(IEditorConfigNode child)
    {
        return this with { Children = Children.Add(child) };
    }
}