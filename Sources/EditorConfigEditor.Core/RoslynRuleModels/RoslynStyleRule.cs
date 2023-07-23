namespace EditorConfigEditor.Core.RoslynRuleModels;

public class RoslynStyleRule
{
    public string RuleId { get; }
    public string Title { get; }
    public string Category { get; }
    public string Overview { get; }
    public string Example { get; }
    public IReadOnlyCollection<RoslynStyleRuleOption> Options { get; }

    public RoslynStyleRule(string ruleId, string title, string category, string overview, string example, IReadOnlyCollection<RoslynStyleRuleOption> options)
    {
        RuleId = ruleId;
        Title = title;
        Category = category;
        Overview = overview;
        Example = example;
        Options = options;
    }
}