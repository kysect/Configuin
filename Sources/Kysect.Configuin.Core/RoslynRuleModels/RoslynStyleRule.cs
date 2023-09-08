namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynStyleRule
{
    public RoslynRuleId RuleId { get; }
    public string Title { get; }
    public string Category { get; }
    public string Overview { get; }
    public string? Example { get; }
    public IReadOnlyCollection<RoslynStyleRuleOption> Options { get; }

    public RoslynStyleRule(RoslynRuleId ruleId, string title, string category, string overview, string? example, IReadOnlyCollection<RoslynStyleRuleOption> options)
    {
        RuleId = ruleId;
        Title = title;
        Category = category;
        Overview = overview;
        Example = example;
        Options = options;
    }
}