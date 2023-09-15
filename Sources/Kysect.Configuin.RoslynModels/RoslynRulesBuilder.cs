namespace Kysect.Configuin.RoslynModels;

public class RoslynRulesBuilder
{
    private readonly List<RoslynQualityRule> _qualityRules;
    private readonly List<RoslynStyleRuleGroup> _styleRuleGroups;

    public RoslynRulesBuilder()
    {
        _qualityRules = new List<RoslynQualityRule>();
        _styleRuleGroups = new List<RoslynStyleRuleGroup>();
    }

    public static RoslynRulesBuilder New()
    {
        return new RoslynRulesBuilder();
    }

    public RoslynRulesBuilder AddQuality(RoslynQualityRule qualityRule)
    {
        _qualityRules.Add(qualityRule);
        return this;
    }

    public RoslynRulesBuilder AddStyle(RoslynStyleRuleGroup styleRuleGroup)
    {
        _styleRuleGroups.Add(styleRuleGroup);
        return this;
    }

    public RoslynRules Build()
    {
        return new RoslynRules(_qualityRules, _styleRuleGroups);
    }
}