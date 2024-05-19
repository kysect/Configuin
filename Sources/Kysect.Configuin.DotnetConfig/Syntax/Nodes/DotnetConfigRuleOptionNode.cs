using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigRuleOptionNode(
    string Key,
    EqualSymbolSyntaxNode EqualSymbol,
    string Value,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia
) : IDotnetConfigPropertySyntaxNode
{
    public DotnetConfigRuleOptionNode(string key, EqualSymbolSyntaxNode equalSymbol, string value) : this(key, equalSymbol, value, [], null)
    {
    }

    public DotnetConfigRuleOptionNode(string key, string value) : this(key, EqualSymbolSyntaxNode.Empty, value, [], null)
    {
    }

    public IDotnetConfigPropertySyntaxNode WithLeadingTrivia(ImmutableList<string> leadingTrivia)
    {
        return this with { LeadingTrivia = leadingTrivia };
    }

    public IDotnetConfigPropertySyntaxNode WithTrailingTrivia(string? trailingTrivia)
    {
        return this with { TrailingTrivia = trailingTrivia };
    }

    public string ToFullString()
    {
        // TODO: reduce duplication
        var fullString = $"{Key}{EqualSymbol.ToFullString()}{Value}";
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        if (TrailingTrivia is not null)
            fullString += $"#{TrailingTrivia}";

        stringBuilder.Append(fullString);
        return stringBuilder.ToString();
    }
}