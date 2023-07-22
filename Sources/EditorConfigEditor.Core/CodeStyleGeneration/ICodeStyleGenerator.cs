using EditorConfigEditor.Core.EditorConfigParsing;
using EditorConfigEditor.Core.RoslynRuleModels;

namespace EditorConfigEditor.Core.CodeStyleGeneration;

public interface ICodeStyleGenerator
{
    CodeStyleInfo Generate(EditorConfigRuleSet editorConfigRuleSet, RoslynRules roslynRules);
}