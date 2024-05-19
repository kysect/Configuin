using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigDocument(ImmutableList<IDotnetConfigSyntaxNode> Children, ImmutableList<string> TrailingTrivia) : IDotnetConfigContainerSyntaxNode
{
    public DotnetConfigDocument() : this([])
    {
    }

    public DotnetConfigDocument(ImmutableList<IDotnetConfigSyntaxNode> children) : this(children, ImmutableList<string>.Empty)
    {
    }

    IDotnetConfigContainerSyntaxNode IDotnetConfigContainerSyntaxNode.AddChild(IDotnetConfigSyntaxNode child)
    {
        return AddChild(child);
    }

    public IDotnetConfigContainerSyntaxNode WithChildren(ImmutableList<IDotnetConfigSyntaxNode> children)
    {
        return this with { Children = children };
    }

    public DotnetConfigDocument AddChild(IDotnetConfigSyntaxNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public string ToFullString()
    {
        var stringBuilder = new StringBuilder();
        List<string> lines = new();
        lines.AddRange(Children.Select(c => c.ToFullString()));
        lines.AddRange(TrailingTrivia);

        stringBuilder.AppendJoin(Environment.NewLine, lines);
        return stringBuilder.ToString();
    }
}