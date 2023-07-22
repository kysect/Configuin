namespace EditorConfigEditor.Core.RoslynRuleModels;

public class EditorConfigStyleRuleOptionValue
{
    public string Value { get; }
    public string Description { get; }

    public EditorConfigStyleRuleOptionValue(string value, string description)
    {
        Value = value;
        Description = description;
    }
}