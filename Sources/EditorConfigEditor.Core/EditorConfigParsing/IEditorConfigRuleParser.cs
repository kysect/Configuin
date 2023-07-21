namespace EditorConfigEditor.Core.EditorConfigParsing;

public interface IEditorConfigRuleParser
{
    EditorConfigRawValues Parse(string content);
}