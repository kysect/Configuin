using Kysect.Configuin.Common;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnPropertyValueDescriptionTable
{
    public IReadOnlyDictionary<string, IReadOnlyList<LearnPropertyValueDescriptionTableRow>> Properties { get; }

    public LearnPropertyValueDescriptionTable(Dictionary<string, IReadOnlyList<LearnPropertyValueDescriptionTableRow>> properties)
    {
        Properties = properties;
    }

    public LearnPropertyValueDescriptionTableRow GetSingleValue(string key)
    {
        IReadOnlyList<LearnPropertyValueDescriptionTableRow> values = GetValues(key);
        if (values.Count == 0)
            throw new ConfiguinException($"Table does not contains value for property {key}");


        if (values.Count > 1)
            throw new ConfiguinException($"Table contains more than one value for property {key}");

        return values[0];
    }

    public IReadOnlyList<LearnPropertyValueDescriptionTableRow> FindValues(string key)
    {
        if (!Properties.TryGetValue(key, out IReadOnlyList<LearnPropertyValueDescriptionTableRow>? value))
            return Array.Empty<LearnPropertyValueDescriptionTableRow>();

        return value;
    }

    public IReadOnlyList<LearnPropertyValueDescriptionTableRow> GetValues(string key)
    {
        if (!Properties.TryGetValue(key, out IReadOnlyList<LearnPropertyValueDescriptionTableRow>? value))
            throw new ConfiguinException($"Table does not contains value for property {key}");

        return value;
    }
}