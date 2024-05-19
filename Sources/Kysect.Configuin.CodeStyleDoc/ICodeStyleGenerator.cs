using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.CodeStyleDoc;

public interface ICodeStyleGenerator
{
    CodeStyle Generate(DotnetConfigDocument dotnetConfigDocument, RoslynRules roslynRules);
}