namespace Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

public class MsLearnPropertyValueDescriptionTable
{
    public IReadOnlyDictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>> Properties { get; }

    public MsLearnPropertyValueDescriptionTable(Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>> properties)
    {
        Properties = properties;
    }
}