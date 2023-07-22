namespace EditorConfigEditor.Core.EditorConfigParsing;

public class EditorConfigRuleSet
{
    public IReadOnlyCollection<IEditorConfigRule> Rows { get; }

    public EditorConfigRuleSet(IReadOnlyCollection<IEditorConfigRule> rows)
    {
        Rows = rows;
    }
}