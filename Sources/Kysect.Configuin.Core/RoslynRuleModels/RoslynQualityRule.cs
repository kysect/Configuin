namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynQualityRule
{
    public RoslynRuleId RuleId { get; }
    public string RuleName { get; }
    public string Category { get; }
    public string Description { get; }

    public RoslynQualityRule(RoslynRuleId ruleId, string ruleName, string category, string description)
    {
        RuleId = ruleId;
        RuleName = ruleName;
        Category = category;
        Description = description;
    }
}