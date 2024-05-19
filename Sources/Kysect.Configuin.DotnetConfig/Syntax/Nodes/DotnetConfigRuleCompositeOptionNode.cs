using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigRuleCompositeOptionNode(
    IReadOnlyCollection<string> KeyParts,
    EqualSymbolSyntaxNode EqualSymbol,
    string Value,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia
    ) : IDotnetConfigPropertySyntaxNode
{
    string IDotnetConfigPropertySyntaxNode.Key => string.Join('.', KeyParts);

    public DotnetConfigRuleCompositeOptionNode(IReadOnlyCollection<string> keyParts, EqualSymbolSyntaxNode equalSymbol, string value) : this(keyParts, equalSymbol, value, [], null)
    {
    }

    public DotnetConfigRuleCompositeOptionNode(IReadOnlyCollection<string> keyParts, string value) : this(keyParts, EqualSymbolSyntaxNode.Empty, value, [], null)
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
        var fullString = $"{string.Join('.', KeyParts)}{EqualSymbol.ToFullString()}{Value}";
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        if (TrailingTrivia is not null)
            fullString += $"#{TrailingTrivia}";

        stringBuilder.Append(fullString);
        return stringBuilder.ToString();
    }
}