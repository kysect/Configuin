using Kysect.Configuin.CodeStyleDoc.Models;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.CodeStyleDoc.Markdown;

public class MarkdownCodeStyleWriter : ICodeStyleWriter
{
    private readonly MarkdownCodeStyleFormatter _formatter;
    private readonly ILogger _logger;

    public MarkdownCodeStyleWriter(ILogger logger)
    {
        _formatter = new MarkdownCodeStyleFormatter(logger);
        _logger = logger;
    }

    public void Write(string filePath, CodeStyle codeStyle)
    {
        string content = _formatter.Format(codeStyle);
        _logger.LogInformation("Writing code style as markdown to file {filePath}", filePath);
        File.WriteAllText(filePath, content);
    }
}