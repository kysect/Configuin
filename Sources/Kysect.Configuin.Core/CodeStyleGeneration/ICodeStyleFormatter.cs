using Kysect.Configuin.Core.CodeStyleGeneration.Models;

namespace Kysect.Configuin.Core.CodeStyleGeneration;

public interface ICodeStyleFormatter
{
    string Format(CodeStyle codeStyle);
}