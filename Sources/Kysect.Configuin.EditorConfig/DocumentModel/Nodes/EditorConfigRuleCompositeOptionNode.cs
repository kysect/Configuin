using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigRuleCompositeOptionNode(
    IReadOnlyCollection<string> KeyParts,
    EqualSymbol EqualSymbol,
    string Value,
    ImmutableList<string> LeadingTrivia,
    string? TrailingTrivia
    ) : IEditorConfigPropertyNode
{
    string IEditorConfigPropertyNode.Key => string.Join('.', KeyParts);

    public EditorConfigRuleCompositeOptionNode(IReadOnlyCollection<string> keyParts, EqualSymbol equalSymbol, string value) : this(keyParts, equalSymbol, value, [], null)
    {
    }

    public EditorConfigRuleCompositeOptionNode(IReadOnlyCollection<string> keyParts, string value) : this(keyParts, EqualSymbol.Empty, value, [], null)
    {
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
        var fullString = $"{string.Join('.', KeyParts)}{EqualSymbol.ToFullString()}{Value}";
        var stringBuilder = new StringBuilder();
        LeadingTrivia.ForEach(s => stringBuilder.AppendLine(s));

        if (TrailingTrivia is not null)
            fullString += $"#{TrailingTrivia}";

        stringBuilder.Append(fullString);
        return stringBuilder.ToString();
    }
}