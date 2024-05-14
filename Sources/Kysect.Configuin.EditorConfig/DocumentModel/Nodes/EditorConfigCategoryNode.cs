using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigCategoryNode(string Value, ImmutableList<IEditorConfigNode> Children, ImmutableList<string> LeadingTrivia, string? TrailingTrivia) : IEditorConfigContainerNode
{
    public EditorConfigCategoryNode(string value) : this(value, ImmutableList<IEditorConfigNode>.Empty, ImmutableList<string>.Empty, null)
    {
    }

    IEditorConfigContainerNode IEditorConfigContainerNode.AddChild(IEditorConfigNode child)
    {
        return AddChild(child);
    }

    public EditorConfigCategoryNode AddChild(IEditorConfigNode child)
    {
        return this with { Children = Children.Add(child) };
    }
}