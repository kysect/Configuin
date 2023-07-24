using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParser.Tables.Models;
using Kysect.Configuin.Core.MarkdownParser.TextExtractor;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MarkdownTableParser
{
    private readonly IMarkdownTextExtractor _textExtractor;

    public MarkdownTableParser(IMarkdownTextExtractor textExtractor)
    {
        _textExtractor = textExtractor;
    }

    public MarkdownTableContent ParseToSimpleContent(Table table)
    {
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

    public MarkdownKeyValueTableContent ParseToKeyValueContent(Table table)
    {
        MarkdownTableContent simpleTable = ParseToSimpleContent(table);

        var values = new Dictionary<string, string>();

        foreach (IReadOnlyList<string> row in simpleTable.Rows)
        {
            if (row.Count != 2)
                throw new ArgumentException($"Unexpected row element count: {row.Count}");

            values[row[0]] = row[1];
        }

        return new MarkdownKeyValueTableContent(simpleTable.Headers, values);
    }

    public MarkdownKeyMultiValueTableContent ParseToMultiValueTable(Table table)
    {
        MarkdownTableContent simpleTable = ParseToSimpleContent(table);

        var rows = new Dictionary<string, IReadOnlyList<string>>();
        string? lastKey = null;
        var values = new List<string>();

        foreach (IReadOnlyList<string> simpleTableRow in simpleTable.Rows)
        {
            // TODO: support case when table have >2 columns
            if (simpleTableRow.Count != 2)
                throw new NotSupportedException($"Only tables with two column is supported in row aggregation by key. Table count: {simpleTableRow.Count}");

            string rowKey = simpleTableRow[0];
            string rowValue = simpleTableRow[1];

            bool continuePreviousKey = string.IsNullOrEmpty(rowKey);

            switch (continuePreviousKey)
            {
                case true when lastKey is null:
                    throw new InvalidOperationException("Table has invalid configuration. Key is missed.");

                case true when lastKey is not null:
                    values.Add(rowValue);
                    break;

                case false when lastKey is null:
                    lastKey = rowKey;
                    values.Add(rowValue);
                    break;

                case false when lastKey is not null:
                    rows[lastKey] = values;
                    lastKey = rowKey;
                    values = new List<string> { rowValue };
                    break;
            }
        }

        if (lastKey is not null)
            rows[lastKey] = values;

        return new MarkdownKeyMultiValueTableContent(simpleTable.Headers, rows);
    }

    // TODO: reduce copy-paste code
    public MarkdownPropertyValueWithDescriptionTable ParsePropertyValueWithDescriptionTable(Table table)
    {
        MarkdownTableContent simpleTable = ParseToSimpleContent(table);

        if (simpleTable.Headers is null)
            throw new ArgumentException("Table must have header for parsing as property-value table");

        if (simpleTable.Headers.Count != 3)
            throw new ArgumentException($"Unexpected column count in property-value table. Expected 3, but was {simpleTable.Headers.Count}");

        string[] expectedHeaders = { "Property", "Value", "Description" };
        for (int i = 0; i < simpleTable.Headers.Count; i++)
        {
            if (simpleTable.Headers[i] != expectedHeaders[i])
                throw new ArgumentException($"Table header on index {i} must be equal to {expectedHeaders[i]} but was {simpleTable.Headers[i]}");
        }

        var rows = new Dictionary<string, IReadOnlyList<MarkdownPropertyValueWithDescriptionTableRow>>();
        string? lastKey = null;
        var values = new List<MarkdownPropertyValueWithDescriptionTableRow>();

        foreach (IReadOnlyList<string> simpleTableRow in simpleTable.Rows)
        {
            string rowKey = simpleTableRow[0];

            bool continuePreviousKey = string.IsNullOrEmpty(rowKey);

            switch (continuePreviousKey)
            {
                case true when lastKey is null:
                    throw new InvalidOperationException("Table has invalid configuration. Key is missed.");

                case true when lastKey is not null:
                    values.Add(new MarkdownPropertyValueWithDescriptionTableRow(simpleTableRow[1], simpleTableRow[2]));
                    break;

                case false when lastKey is null:
                    lastKey = rowKey;
                    values.Add(new MarkdownPropertyValueWithDescriptionTableRow(simpleTableRow[1], simpleTableRow[2]));
                    break;

                case false when lastKey is not null:
                    rows[lastKey] = values;
                    lastKey = rowKey;
                    values = new List<MarkdownPropertyValueWithDescriptionTableRow> { new(simpleTableRow[1], simpleTableRow[2]) };
                    break;
            }
        }

        if (lastKey is not null)
            rows[lastKey] = values;

        return new MarkdownPropertyValueWithDescriptionTable(rows);
    }
}