using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Markdown.Tables;

public class MarkdownTableParser
{
    private readonly IMarkdownTextExtractor _textExtractor;

    public MarkdownTableParser(IMarkdownTextExtractor textExtractor)
    {
        _textExtractor = textExtractor;
    }

    public MarkdownTableContent ParseToSimpleContent(Table table)
    {
        table.ThrowIfNull();

        List<string>? headers = null;
        var parsedRows = new List<IReadOnlyList<string>>();

        foreach (Block block in table)
        {
            TableRow row = block.To<TableRow>();

            var values = row
                .Cast<TableCell>()
                .Select(_textExtractor.ExtractText)
                .ToList();

            if (row.IsHeader)
            {
                headers = values;
            }
            else
            {
                parsedRows.Add(values);
            }
        }

        return new MarkdownTableContent(headers, parsedRows);
    }
}