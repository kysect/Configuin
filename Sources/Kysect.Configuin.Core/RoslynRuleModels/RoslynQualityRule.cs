namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynQualityRule
{
    public RoslynRuleId RuleId { get; }
    public string Title { get; }
    public string Category { get; }
    public string Description { get; }

    public RoslynQualityRule(RoslynRuleId ruleId, string title, string category, string description)
    {
        RuleId = ruleId;
        Title = title;
        Category = category;
        Description = description;
    }
}