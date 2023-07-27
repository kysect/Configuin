namespace Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

public class MsLearnKeyValueTableContent
{
    public IReadOnlyList<string>? Headers { get; }
    public IReadOnlyDictionary<string, string> Rows { get; }

    public MsLearnKeyValueTableContent(IReadOnlyList<string>? headers, IReadOnlyDictionary<string, string> rows)
    {
        Headers = headers;
        Rows = rows;
    }
}