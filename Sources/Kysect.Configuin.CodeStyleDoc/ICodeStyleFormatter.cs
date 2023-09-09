using Kysect.Configuin.CodeStyleDoc.Models;

namespace Kysect.Configuin.CodeStyleDoc;

public interface ICodeStyleFormatter
{
    string Format(CodeStyle codeStyle);
}