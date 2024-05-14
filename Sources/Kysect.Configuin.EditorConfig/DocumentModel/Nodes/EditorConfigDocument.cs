using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigDocument(ImmutableList<IEditorConfigNode> Children) : IEditorConfigContainerNode
{
    IEditorConfigContainerNode IEditorConfigContainerNode.AddChild(IEditorConfigNode child)
    {
        return new EditorConfigDocument(Children: Children.Add(child));
    }

    public EditorConfigDocument AddChild(IEditorConfigNode child)
    {
        return new EditorConfigDocument(Children: Children.Add(child));
    }
}