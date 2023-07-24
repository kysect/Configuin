namespace Kysect.Configuin.Core.MarkdownParser.Tables.Models;

public class MarkdownKeyMultiValueTableContent
{
    public IReadOnlyList<string>? Headers { get; }
    public IReadOnlyDictionary<string, IReadOnlyList<string>> Rows { get; }

    public MarkdownKeyMultiValueTableContent(IReadOnlyList<string>? headers, IReadOnlyDictionary<string, IReadOnlyList<string>> rows)
    {
        Headers = headers;
        Rows = rows;
    }
}