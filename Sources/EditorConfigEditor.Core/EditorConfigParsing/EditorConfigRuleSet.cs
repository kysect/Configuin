namespace EditorConfigEditor.Core.EditorConfigParsing;

public class EditorConfigRuleSet
{
    public IReadOnlyCollection<IEditorConfigRule> Rules { get; }

    public EditorConfigRuleSet(IReadOnlyCollection<IEditorConfigRule> rules)
    {
        Rules = rules;
    }
}