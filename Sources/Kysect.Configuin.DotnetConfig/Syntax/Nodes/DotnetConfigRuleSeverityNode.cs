using Kysect.Configuin.RoslynModels;
using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record DotnetConfigRuleSeverityNode(
    RoslynRuleId RuleId,
    EqualSymbolSyntaxNode EqualSymbol,
    string Severity,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia) : IDotnetConfigPropertySyntaxNode
{
    string IDotnetConfigPropertySyntaxNode.Key => $"dotnet_diagnostic.{RuleId}.severity";
    string IDotnetConfigPropertySyntaxNode.Value => Severity.ToString();

    public DotnetConfigRuleSeverityNode(RoslynRuleId ruleId, EqualSymbolSyntaxNode equalSymbol, string severity) : this(ruleId, equalSymbol, severity, [], null)
    {
    }

    public DotnetConfigRuleSeverityNode(RoslynRuleId ruleId, string severity) : this(ruleId, EqualSymbolSyntaxNode.Empty, severity, [], null)
    {
    }

    public RoslynRuleSeverity ParseSeverity()
    {
        return Enum.Parse<RoslynRuleSeverity>(Severity, ignoreCase: true);
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
        var fullString = $"dotnet_diagnostic.{RuleId}.severity{EqualSymbol.ToFullString()}{Severity}";
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        if (TrailingTrivia is not null)
            fullString += $"#{TrailingTrivia}";

        stringBuilder.Append(fullString);
        return stringBuilder.ToString();
    }
}