namespace EditorConfigEditor.Core.EditorConfigModels;

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