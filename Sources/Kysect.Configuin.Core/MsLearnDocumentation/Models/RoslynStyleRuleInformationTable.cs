using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.MsLearnDocumentation.Models;

public record RoslynStyleRuleInformationTable(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Subcategory,
    string ApplicableLanguages,
    IReadOnlyCollection<string> Options);