namespace Kysect.Configuin.RoslynModels;

public record RoslynRules(
    IReadOnlyCollection<RoslynQualityRule> QualityRules,
    IReadOnlyCollection<RoslynStyleRuleGroup> StyleRuleGroups)
{
    public IReadOnlyCollection<RoslynStyleRule> GetStyleRules()
    {
        return StyleRuleGroups.SelectMany(r => r.Rules).ToList();
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> GetOptions()
    {
        return StyleRuleGroups
            .SelectMany(r => r.Options)
            // TODO: check duplicates
            .DistinctBy(o => o.Name)
            .ToList();
    }
}