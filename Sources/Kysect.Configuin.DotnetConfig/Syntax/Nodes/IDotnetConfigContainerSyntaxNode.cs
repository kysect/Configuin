using System.Collections.Immutable;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public interface IDotnetConfigContainerSyntaxNode : IDotnetConfigSyntaxNode
{
    ImmutableList<IDotnetConfigSyntaxNode> Children { get; }
    IDotnetConfigContainerSyntaxNode AddChild(IDotnetConfigSyntaxNode child);
    IDotnetConfigContainerSyntaxNode WithChildren(ImmutableList<IDotnetConfigSyntaxNode> children);
}