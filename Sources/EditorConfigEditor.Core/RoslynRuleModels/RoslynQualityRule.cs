namespace EditorConfigEditor.Core.RoslynRuleModels;

public class RoslynQualityRule
{
    public string RuleId { get; }
    public string RuleName { get; }
    public string Category { get; }
    public string Description { get; }

    public RoslynQualityRule(string ruleId, string ruleName, string category, string description)
    {
        RuleId = ruleId;
        RuleName = ruleName;
        Category = category;
        Description = description;
    }
}