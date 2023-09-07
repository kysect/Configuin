using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Markdown;

public class MarkdownCodeStyleWriter : ICodeStyleWriter
{
    private readonly string _filePath;
    private readonly ILogger _logger;

    private readonly MarkdownCodeStyleFormatter _formatter;

    public MarkdownCodeStyleWriter(string filePath, ILogger logger)
    {
        _formatter = new MarkdownCodeStyleFormatter(logger);
        _filePath = filePath;
        _logger = logger;
    }

    public void Write(CodeStyle codeStyle)
    {
        string content = _formatter.Format(codeStyle);
        _logger.LogInformation("Writing code style as markdown to file {filePath}", _filePath);
        File.WriteAllText(_filePath, content);
    }
}