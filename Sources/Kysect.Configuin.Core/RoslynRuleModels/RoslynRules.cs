namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynRules
{
    public IReadOnlyCollection<RoslynQualityRule> QualityRules { get; }
    public IReadOnlyCollection<RoslynStyleRule> StyleRules { get; }

    public RoslynRules(IReadOnlyCollection<RoslynQualityRule> qualityRules, IReadOnlyCollection<RoslynStyleRule> styleRules)
    {
        QualityRules = qualityRules;
        StyleRules = styleRules;
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> GetOptions()
    {
        return StyleRules
            .SelectMany(r => r.Options)
            .ToList();
    }
}