namespace Kysect.Configuin.Core.MarkdownParser.Tables.Models;

public class MarkdownPropertyValueWithDescriptionTable
{
    public Dictionary<string, IReadOnlyList<MarkdownPropertyValueWithDescriptionTableRow>> Rows { get; }

    public MarkdownPropertyValueWithDescriptionTable(Dictionary<string, IReadOnlyList<MarkdownPropertyValueWithDescriptionTableRow>> rows)
    {
        Rows = rows;
    }
}