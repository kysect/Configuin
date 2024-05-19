using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigCategoryNode(
    string Value,
    ImmutableList<IDotnetConfigSyntaxNode> Children,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia
    ) : IDotnetConfigContainerSyntaxNode
{
    public DotnetConfigCategoryNode(string value) : this(value, ImmutableList<IDotnetConfigSyntaxNode>.Empty, ImmutableList<string>.Empty, null)
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

    public DotnetConfigCategoryNode AddChild(IDotnetConfigSyntaxNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public IDotnetConfigSyntaxNode WithLeadingTrivia(ImmutableList<string> leadingTrivia)
    {
        return this with { LeadingTrivia = leadingTrivia };
    }

    public IDotnetConfigSyntaxNode WithTrailingTrivia(string? trailingTrivia)
    {
        return this with { TrailingTrivia = trailingTrivia };
    }

    public string ToFullString()
    {
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        string line = $"[{Value}]";
        if (TrailingTrivia is not null)
            line += $"{line} {TrailingTrivia}";
        stringBuilder.Append(line);

        Children.ForEach(c => stringBuilder.Append($"{Environment.NewLine}{c.ToFullString()}"));
        return stringBuilder.ToString();
    }
}