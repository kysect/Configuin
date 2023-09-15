namespace Kysect.Configuin.RoslynModels;

public class RoslynRules
{
    public IReadOnlyCollection<RoslynQualityRule> QualityRules { get; }
    public IReadOnlyCollection<RoslynStyleRule> StyleRules { get; }
    public IReadOnlyCollection<RoslynStyleRuleGroup> StyleRuleGroups { get; }

    public RoslynRules(
        IReadOnlyCollection<RoslynQualityRule> qualityRules,
        IReadOnlyCollection<RoslynStyleRuleGroup> styleRuleGroups)
    {
        QualityRules = qualityRules;
        StyleRuleGroups = styleRuleGroups;

        StyleRules = styleRuleGroups.SelectMany(r => r.Rules).ToList();
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> GetOptions()
    {
        return StyleRules
            .SelectMany(r => r.Options)
            // TODO: check duplicates
            .DistinctBy(o => o.Name)
            .ToList();
    }
}