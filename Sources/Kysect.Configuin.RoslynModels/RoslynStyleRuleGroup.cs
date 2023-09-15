namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRuleGroup(
    IReadOnlyCollection<RoslynStyleRule> Rules,
    IReadOnlyCollection<RoslynStyleRuleOption> Options,
    string Overview,
    string? Example)
{
    public RoslynStyleRuleGroup(
        RoslynStyleRule rule,
        string Overview,
        string? Example)
        : this(new[] { rule }, Array.Empty<RoslynStyleRuleOption>(), Overview, Example)
    {
    }

    public RoslynStyleRuleGroup(
        RoslynStyleRule rule,
        IReadOnlyCollection<RoslynStyleRuleOption> Options,
        string Overview,
        string? Example)
        : this(new[] { rule }, Options, Overview, Example)
    {
    }
}