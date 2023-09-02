using System.Text;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Markdown;

public class MarkdownStringBuilder
{
    private readonly StringBuilder _builder = new StringBuilder();

    public MarkdownStringBuilder AddEmptyLine()
    {
        _builder.AppendLine();
        return this;
    }

    public MarkdownStringBuilder AddH2(string header)
    {
        _builder.AppendLine($"## {header}");
        return this;
    }

    public MarkdownStringBuilder AddH3(string header)
    {
        _builder.AppendLine($"### {header}");
        return this;
    }

    public MarkdownStringBuilder AddText(string text)
    {
        _builder.AppendLine(text);
        return this;
    }

    public MarkdownStringBuilder AddCode(string text)
    {
        _builder.AppendLine("```csharp");
        _builder.AppendLine(text);
        _builder.AppendLine("```");
        return this;
    }

    public string Build()
    {
        return _builder.ToString();
    }
}