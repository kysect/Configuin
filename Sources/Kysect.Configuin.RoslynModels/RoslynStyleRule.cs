namespace Kysect.Configuin.RoslynModels;

// TODO: maybe move overview and example to RuleGroup?
public record RoslynStyleRule(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Overview,
    string? Example);