namespace Kysect.Configuin.RoslynModels;

public record RoslynQualityRule(RoslynRuleId RuleId, string Title, string Category, string Description, IReadOnlyCollection<string> Options);