using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration;

public interface ICodeStyleGenerator
{
    CodeStyle Generate(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules);
}