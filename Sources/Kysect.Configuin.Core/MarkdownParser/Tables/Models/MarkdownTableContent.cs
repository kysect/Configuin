namespace Kysect.Configuin.Core.MarkdownParser.Tables.Models;

public class MarkdownTableContent
{
    public IReadOnlyList<string>? Headers { get; }
    public IReadOnlyList<IReadOnlyList<string>> Rows { get; }

    public MarkdownTableContent(IReadOnlyList<string>? headers, IReadOnlyList<IReadOnlyList<string>> rows)
    {
        Headers = headers;
        Rows = rows;
    }
}