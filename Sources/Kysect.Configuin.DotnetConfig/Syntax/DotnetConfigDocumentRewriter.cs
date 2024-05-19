using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.DotnetConfig.Syntax;

public static class DotnetConfigDocumentRewriter
{
    public static DotnetConfigDocument RemoveNodes(this DotnetConfigDocument document, IReadOnlyCollection<IDotnetConfigSyntaxNode> nodes)
    {
        return (DotnetConfigDocument) document.FilterChildren(nodes);
    }

    public static DotnetConfigDocument ReplaceNodes(this DotnetConfigDocument document, IDotnetConfigSyntaxNode oldSyntaxNode, IDotnetConfigSyntaxNode newSyntaxNode)
    {
        return (DotnetConfigDocument) document.ReplaceChildren(oldSyntaxNode, newSyntaxNode);
    }

    public static DotnetConfigDocument UpdateNodes(this DotnetConfigDocument document, Func<IDotnetConfigSyntaxNode, IDotnetConfigSyntaxNode> morphimsm)
    {
        document.ThrowIfNull();
        morphimsm.ThrowIfNull();

        return (DotnetConfigDocument) document.UpdateChildren(morphimsm);
    }


    private static IDotnetConfigSyntaxNode FilterChildren(this IDotnetConfigSyntaxNode syntaxNode, IReadOnlyCollection<IDotnetConfigSyntaxNode> nodes)
    {
        if (syntaxNode is not IDotnetConfigContainerSyntaxNode containerNode)
            return syntaxNode;

        ImmutableList<IDotnetConfigSyntaxNode> newChildren = [];
        foreach (IDotnetConfigSyntaxNode editorConfigNode in containerNode.Children)
        {
            if (nodes.Contains(editorConfigNode))
                continue;

            newChildren = newChildren.Add(editorConfigNode.FilterChildren(nodes));
        }

        return containerNode.WithChildren(newChildren);
    }

    private static IDotnetConfigSyntaxNode ReplaceChildren(this IDotnetConfigSyntaxNode currentSyntaxNode, IDotnetConfigSyntaxNode oldSyntaxNode, IDotnetConfigSyntaxNode newSyntaxNode)
    {
        if (currentSyntaxNode == oldSyntaxNode)
            return newSyntaxNode;

        if (currentSyntaxNode is not IDotnetConfigContainerSyntaxNode containerNode)
            return currentSyntaxNode;

        ImmutableList<IDotnetConfigSyntaxNode> newChildren = [];
        foreach (IDotnetConfigSyntaxNode editorConfigNode in containerNode.Children)
            newChildren = newChildren.Add(editorConfigNode.ReplaceChildren(oldSyntaxNode, newSyntaxNode));

        return containerNode.WithChildren(newChildren);
    }

    private static IDotnetConfigSyntaxNode UpdateChildren(this IDotnetConfigSyntaxNode currentSyntaxNode, Func<IDotnetConfigSyntaxNode, IDotnetConfigSyntaxNode> morphimsm)
    {
        currentSyntaxNode = morphimsm(currentSyntaxNode);

        if (currentSyntaxNode is not IDotnetConfigContainerSyntaxNode containerNode)
            return currentSyntaxNode;

        ImmutableList<IDotnetConfigSyntaxNode> newChildren = containerNode
            .Children
            .Select(morphimsm)
            .ToImmutableList();
        return containerNode.WithChildren(newChildren);
    }
}