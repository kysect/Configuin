namespace Kysect.Configuin.Core.EditorConfigParsing;

public interface IEditorConfigRuleParser
{
    EditorConfigRuleSet Parse(string content);
}