using Kysect.Configuin.Core.CodeStyleGeneration.Models;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Markdown;

public class MarkdownCodeStyleWriter : ICodeStyleWriter
{
    private readonly MarkdownCodeStyleFormatter _formatter = new MarkdownCodeStyleFormatter();

    private readonly string _filePath;

    public MarkdownCodeStyleWriter(string filePath)
    {
        _filePath = filePath;
    }

    public void Write(CodeStyle codeStyle)
    {
        string content = _formatter.Format(codeStyle);
        File.WriteAllText(_filePath, content);
    }
}