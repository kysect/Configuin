namespace Kysect.Configuin.MsLearn.Tables.Models;

public record MsLearnPropertyValueDescriptionTableRow(string Value, string Description)
{
    public MsLearnPropertyValueDescriptionTableRow(string Value) : this(Value, string.Empty)
    {
    }
};