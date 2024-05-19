using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;

namespace Kysect.Configuin.DotnetConfig.Syntax;

public static class DotnetConfigSyntaxNodeExtensions
{
    public static IReadOnlyCollection<IDotnetConfigSyntaxNode> DescendantNodes(this IDotnetConfigSyntaxNode syntaxNode)
    {
        syntaxNode.ThrowIfNull();

        List<IDotnetConfigSyntaxNode> result =
        [
            syntaxNode
        ];

        if (syntaxNode is IDotnetConfigContainerSyntaxNode configContainer)
        {
            foreach (IDotnetConfigSyntaxNode child in configContainer.Children)
            {
                result.AddRange(child.DescendantNodes());
            }
        }

        return result;
    }
}