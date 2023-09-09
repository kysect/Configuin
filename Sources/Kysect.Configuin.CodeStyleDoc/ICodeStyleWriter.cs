using Kysect.Configuin.CodeStyleDoc.Models;

namespace Kysect.Configuin.CodeStyleDoc;

public interface ICodeStyleWriter
{
    void Write(CodeStyle codeStyle);
}