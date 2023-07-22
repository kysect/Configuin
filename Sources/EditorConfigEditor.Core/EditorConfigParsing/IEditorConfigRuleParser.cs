namespace EditorConfigEditor.Core.EditorConfigParsing;

public interface IEditorConfigRuleParser
{
    EditorConfigRuleSet Parse(string content);
}