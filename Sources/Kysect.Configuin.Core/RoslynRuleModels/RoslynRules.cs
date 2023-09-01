namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynRules
{
    public IReadOnlyCollection<RoslynQualityRule> QualityRules { get; }
    public IReadOnlyCollection<RoslynStyleRule> StyleRules { get; }
    public IReadOnlyCollection<RoslynStyleRuleOption> DotnetFormattingOptions { get; }
    public IReadOnlyCollection<RoslynStyleRuleOption> SharpFormattingOptions { get; }

    public RoslynRules(
        IReadOnlyCollection<RoslynQualityRule> qualityRules,
        IReadOnlyCollection<RoslynStyleRule> styleRules,
        IReadOnlyCollection<RoslynStyleRuleOption> dotnetFormattingOptions,
        IReadOnlyCollection<RoslynStyleRuleOption> sharpFormattingOptions)
    {
        QualityRules = qualityRules;
        StyleRules = styleRules;
        DotnetFormattingOptions = dotnetFormattingOptions;
        SharpFormattingOptions = sharpFormattingOptions;
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> GetOptions()
    {
        return StyleRules
            .SelectMany(r => r.Options)
            .ToList();
    }
}