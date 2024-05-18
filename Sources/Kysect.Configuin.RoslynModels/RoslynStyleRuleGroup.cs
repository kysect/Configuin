namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRuleGroup(
    IReadOnlyCollection<RoslynStyleRule> Rules,
    IReadOnlyCollection<RoslynStyleRuleOption> Options,
    string Overview,
    string? Example);