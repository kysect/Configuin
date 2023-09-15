namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRuleGroup(IReadOnlyCollection<RoslynStyleRule> Rules, IReadOnlyCollection<RoslynStyleRuleOption> Options)
{
    public RoslynStyleRuleGroup(RoslynStyleRule rule) : this(new[] { rule }, Array.Empty<RoslynStyleRuleOption>())
    {
    }

    public RoslynStyleRuleGroup(RoslynStyleRule rule, IReadOnlyCollection<RoslynStyleRuleOption> Options) : this(new[] { rule }, Options)
    {
    }
}