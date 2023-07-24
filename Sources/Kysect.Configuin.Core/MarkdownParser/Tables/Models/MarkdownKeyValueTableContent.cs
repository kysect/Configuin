namespace Kysect.Configuin.Core.MarkdownParser.Tables.Models;

public class MarkdownKeyValueTableContent
{
    public IReadOnlyList<string>? Headers { get; }
    public IReadOnlyDictionary<string, string> Rows { get; }

    public MarkdownKeyValueTableContent(IReadOnlyList<string>? headers, IReadOnlyDictionary<string, string> rows)
    {
        Headers = headers;
        Rows = rows;
    }
}