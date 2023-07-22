namespace EditorConfigEditor.Core.RoslynRuleModels;

public class RoslynStyleRuleOption
{
    public string Name { get; }
    public IReadOnlyCollection<EditorConfigStyleRuleOptionValue> Options { get; }

    public RoslynStyleRuleOption(string name, IReadOnlyCollection<EditorConfigStyleRuleOptionValue> options)
    {
        Name = name;
        Options = options;
    }
}