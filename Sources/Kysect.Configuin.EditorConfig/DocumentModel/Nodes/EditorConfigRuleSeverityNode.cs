using Kysect.Configuin.RoslynModels;
using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigRuleSeverityNode(
    RoslynRuleId RuleId,
    EqualSymbol EqualSymbol,
    string Severity,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia) : IEditorConfigPropertyNode
{
    string IEditorConfigPropertyNode.Key => $"dotnet_diagnostic.{RuleId}.severity";
    string IEditorConfigPropertyNode.Value => Severity.ToString();

    public EditorConfigRuleSeverityNode(RoslynRuleId ruleId, EqualSymbol equalSymbol, string severity) : this(ruleId, equalSymbol, severity, [], null)
    {
    }

    public EditorConfigRuleSeverityNode(RoslynRuleId ruleId, string severity) : this(ruleId, EqualSymbol.Empty, severity, [], null)
    {
    }

    public RoslynRuleSeverity ParseSeverity()
    {
        return Enum.Parse<RoslynRuleSeverity>(Severity, ignoreCase: true);
    }

    public IEditorConfigPropertyNode WithLeadingTrivia(ImmutableList<string> leadingTrivia)
    {
        return this with { LeadingTrivia = leadingTrivia };
    }

    public IEditorConfigPropertyNode WithTrailingTrivia(string? trailingTrivia)
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