namespace Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

public record MsLearnPropertyValueDescriptionTableRow(string Value, string Description)
{
    public MsLearnPropertyValueDescriptionTableRow(string Value) : this(Value, string.Empty)
    {
    }
};