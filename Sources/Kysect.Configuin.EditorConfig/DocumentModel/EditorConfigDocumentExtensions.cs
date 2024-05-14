using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public static class EditorConfigDocumentExtensions
{
    public static IReadOnlyCollection<IEditorConfigNode> DescendantNodes(this IEditorConfigNode node)
    {
        node.ThrowIfNull();

        List<IEditorConfigNode> result =
        [
            node
        ];

        if (node is IEditorConfigContainerNode configContainer)
        {
            foreach (IEditorConfigNode child in configContainer.Children)
            {
                result.AddRange(child.DescendantNodes());
            }
        }

        return result;
    }
}