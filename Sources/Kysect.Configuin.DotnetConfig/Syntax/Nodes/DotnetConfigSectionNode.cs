using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigSectionNode(string Value, ImmutableList<IDotnetConfigSyntaxNode> Children, ImmutableList<string> LeadingTrivia, string? TrailingTrivia) : IDotnetConfigContainerSyntaxNode
{
    public const string NodeIndicator = "###";

    public DotnetConfigSectionNode(string value) : this(value, ImmutableList<IDotnetConfigSyntaxNode>.Empty, ImmutableList<string>.Empty, null)
    {
    }

    IDotnetConfigContainerSyntaxNode IDotnetConfigContainerSyntaxNode.AddChild(IDotnetConfigSyntaxNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public IDotnetConfigContainerSyntaxNode WithChildren(ImmutableList<IDotnetConfigSyntaxNode> children)
    {
        return this with { Children = children };
    }

    public DotnetConfigSectionNode AddChild(IDotnetConfigSyntaxNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public string ToFullString()
    {
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        string line = $"{Value}";
        if (TrailingTrivia is not null)
            line += $"{line} {TrailingTrivia}";
        stringBuilder.Append(line);

        Children.ForEach(c => stringBuilder.Append($"{Environment.NewLine}{c.ToFullString()}"));
        return stringBuilder.ToString();
    }
}