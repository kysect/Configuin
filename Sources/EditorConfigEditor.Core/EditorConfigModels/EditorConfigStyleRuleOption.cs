namespace EditorConfigEditor.Core.EditorConfigModels;

public class EditorConfigStyleRuleOption
{
    public string Name { get; }
    public IReadOnlyCollection<EditorConfigStyleRuleOptionValue> Options { get; }

    public EditorConfigStyleRuleOption(string name, IReadOnlyCollection<EditorConfigStyleRuleOptionValue> options)
    {
        Name = name;
        Options = options;
    }
}