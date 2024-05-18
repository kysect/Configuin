namespace Kysect.Configuin.Learn.ContentParsing;

public record LearnPropertyValueDescriptionTableRow(string Value, string Description)
{
    public LearnPropertyValueDescriptionTableRow(string Value) : this(Value, string.Empty)
    {
    }
};