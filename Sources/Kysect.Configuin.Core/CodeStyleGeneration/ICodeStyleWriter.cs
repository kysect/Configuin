using Kysect.Configuin.Core.CodeStyleGeneration.Models;

namespace Kysect.Configuin.Core.CodeStyleGeneration;

public interface ICodeStyleWriter
{
    void Write(CodeStyleInfo codeStyleInfo);
}