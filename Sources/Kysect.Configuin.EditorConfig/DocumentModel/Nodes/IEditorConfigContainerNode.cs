using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public interface IEditorConfigContainerNode : IEditorConfigNode
{
    ImmutableList<IEditorConfigNode> Children { get; }
    IEditorConfigContainerNode AddChild(IEditorConfigNode child);
}