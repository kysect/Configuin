using Kysect.Configuin.Core.MarkdownParsing.Tables.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

namespace Kysect.Configuin.Core.MsLearnDocumentation.Tables;

public class MsLearnTableParser
{
    public MsLearnPropertyValueDescriptionTable Parse(MarkdownTableContent simpleTable)
    {
        ValidateTableHeader(simpleTable);

        var rows = new Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>>();
        string? lastKey = null;
        var values = new List<MsLearnPropertyValueDescriptionTableRow>();

        foreach (IReadOnlyList<string> simpleTableRow in simpleTable.Rows)
        {
            string rowKey = simpleTableRow[0];
            string value = simpleTableRow[1];
            string? description = simpleTableRow.Count < 3 ? string.Empty : simpleTableRow[2];

            bool continuePreviousKey = string.IsNullOrEmpty(rowKey);

            switch (continuePreviousKey)
            {
                case true when lastKey is null:
                    throw new InvalidOperationException("Table has invalid configuration. Key is missed.");

                case true when lastKey is not null:
                    values.Add(new MsLearnPropertyValueDescriptionTableRow(value, description));
                    break;

                case false when lastKey is null:
                    lastKey = rowKey;
                    values.Add(new MsLearnPropertyValueDescriptionTableRow(value, description));
                    break;

                case false when lastKey is not null:
                    rows[lastKey] = values;
                    lastKey = rowKey;
                    values = new List<MsLearnPropertyValueDescriptionTableRow> { new(value, description) };
                    break;
            }
        }

        if (lastKey is not null)
            rows[lastKey] = values;

        return new MsLearnPropertyValueDescriptionTable(rows);
    }

    private static void ValidateTableHeader(MarkdownTableContent simpleTable)
    {
        if (simpleTable.Headers is null)
            throw new ArgumentException("Table must have header for parsing as property-value table");

        if (simpleTable.Headers.Count != 2 && simpleTable.Headers.Count != 3)
            throw new ArgumentException($"Unexpected column count in property-value table. Expected 2 or 3, but was {simpleTable.Headers.Count}");

        string[] expectedHeaders = { "Property", "Value", "Description" };
        for (int i = 0; i < simpleTable.Headers.Count; i++)
        {
            if (simpleTable.Headers[i] != expectedHeaders[i])
                throw new ArgumentException($"Table header on index {i} must be equal to {expectedHeaders[i]} but was {simpleTable.Headers[i]}");
        }
    }
}