using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public static class EditorConfigDocumentNodeRewriter
{
    public static EditorConfigDocument RemoveNodes(this EditorConfigDocument document, IReadOnlyCollection<IEditorConfigNode> nodes)
    {
        return (EditorConfigDocument) document.FilterChildren(nodes);
    }

    public static EditorConfigDocument ReplaceNodes(this EditorConfigDocument document, IEditorConfigNode oldNode, IEditorConfigNode newNode)
    {
        return (EditorConfigDocument) document.ReplaceChildren(oldNode, newNode);
    }

    public static EditorConfigDocument UpdateNodes(this EditorConfigDocument document, Func<IEditorConfigNode, IEditorConfigNode> morphimsm)
    {
        document.ThrowIfNull();
        morphimsm.ThrowIfNull();

        return (EditorConfigDocument) document.UpdateChildren(morphimsm);
    }


    private static IEditorConfigNode FilterChildren(this IEditorConfigNode node, IReadOnlyCollection<IEditorConfigNode> nodes)
    {
        if (node is not IEditorConfigContainerNode containerNode)
            return node;

        ImmutableList<IEditorConfigNode> newChildren = [];
        foreach (IEditorConfigNode editorConfigNode in containerNode.Children)
        {
            if (nodes.Contains(editorConfigNode))
                continue;

            newChildren = newChildren.Add(editorConfigNode.FilterChildren(nodes));
        }

        return containerNode.WithChildren(newChildren);
    }

    private static IEditorConfigNode ReplaceChildren(this IEditorConfigNode currentNode, IEditorConfigNode oldNode, IEditorConfigNode newNode)
    {
        if (currentNode == oldNode)
            return newNode;

        if (currentNode is not IEditorConfigContainerNode containerNode)
            return currentNode;

        ImmutableList<IEditorConfigNode> newChildren = [];
        foreach (IEditorConfigNode editorConfigNode in containerNode.Children)
            newChildren = newChildren.Add(editorConfigNode.ReplaceChildren(oldNode, newNode));

        return containerNode.WithChildren(newChildren);
    }

    private static IEditorConfigNode UpdateChildren(this IEditorConfigNode currentNode, Func<IEditorConfigNode, IEditorConfigNode> morphimsm)
    {
        currentNode = morphimsm(currentNode);

        if (currentNode is not IEditorConfigContainerNode containerNode)
            return currentNode;

        ImmutableList<IEditorConfigNode> newChildren = containerNode
            .Children
            .Select(morphimsm)
            .ToImmutableList();
        return containerNode.WithChildren(newChildren);
    }
}