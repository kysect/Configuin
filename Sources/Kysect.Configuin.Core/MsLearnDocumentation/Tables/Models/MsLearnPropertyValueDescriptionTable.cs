using Kysect.Configuin.Common;

namespace Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

public class MsLearnPropertyValueDescriptionTable
{
    public IReadOnlyDictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>> Properties { get; }

    public MsLearnPropertyValueDescriptionTable(Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>> properties)
    {
        Properties = properties;
    }

    public MsLearnPropertyValueDescriptionTableRow GetSingleValue(string key)
    {
        IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> values = GetValues(key);
        if (values.Count == 0)
            throw new ConfiguinException($"Table does not contains value for property {key}");


        if (values.Count > 1)
            throw new ConfiguinException($"Table contains more than one value for property {key}");

        return values[0];
    }

    public IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> FindValues(string key)
    {
        if (!Properties.TryGetValue(key, out IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>? value))
            return Array.Empty<MsLearnPropertyValueDescriptionTableRow>();

        return value;
    }

    public IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> GetValues(string key)
    {
        if (!Properties.TryGetValue(key, out IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>? value))
            throw new ConfiguinException($"Table does not contains value for property {key}");

        return value;
    }
}