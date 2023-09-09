namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRule(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Overview,
    string? Example,
    IReadOnlyCollection<RoslynStyleRuleOption> Options);