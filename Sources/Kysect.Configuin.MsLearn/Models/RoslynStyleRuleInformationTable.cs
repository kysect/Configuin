using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.MsLearn.Models;

public record RoslynStyleRuleInformationTable(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Subcategory,
    string ApplicableLanguages,
    IReadOnlyCollection<string> Options);