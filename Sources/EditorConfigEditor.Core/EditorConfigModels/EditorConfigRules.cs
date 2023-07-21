namespace EditorConfigEditor.Core.EditorConfigModels;

public class EditorConfigRules
{
    public IReadOnlyCollection<EditorConfigQualityRule> QualityRules { get; }
    public IReadOnlyCollection<EditorConfigStyleRule> StyleRules { get; }

    public EditorConfigRules(IReadOnlyCollection<EditorConfigQualityRule> qualityRules, IReadOnlyCollection<EditorConfigStyleRule> styleRules)
    {
        QualityRules = qualityRules;
        StyleRules = styleRules;
    }
}