using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration;

public interface ICodeStyleGenerator
{
    CodeStyleInfo Generate(EditorConfigRuleSet editorConfigRuleSet, RoslynRules roslynRules);
}