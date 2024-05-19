using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.CodeStyleDoc;

public interface ICodeStyleGenerator
{
    CodeStyle Generate(EditorConfigDocument editorConfigDocument, RoslynRules roslynRules);
}