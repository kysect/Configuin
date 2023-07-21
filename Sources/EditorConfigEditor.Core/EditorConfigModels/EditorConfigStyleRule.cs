namespace EditorConfigEditor.Core.EditorConfigModels;

public class EditorConfigStyleRule
{
    public string RuleId { get; }
    public string Title { get; }
    public string Category { get; }
    public string Overview { get; }
    public string Example { get; }
    public IReadOnlyCollection<EditorConfigStyleRuleOption> Options { get; }

    public EditorConfigStyleRule(string ruleId, string title, string category, string overview, string example, IReadOnlyCollection<EditorConfigStyleRuleOption> options)
    {
        RuleId = ruleId;
        Title = title;
        Category = category;
        Overview = overview;
        Example = example;
        Options = options;
    }
}