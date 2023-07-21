using EditorConfigEditor.Core.EditorConfigModels;
using EditorConfigEditor.Core.EditorConfigParsing;

namespace EditorConfigEditor.Core.CodeStyleGeneration;

public interface ICodeStyleGenerator
{
    CodeStyleInfo Generate(EditorConfigRawValues editorConfigRawValues, EditorConfigRules editorConfigRules);
}