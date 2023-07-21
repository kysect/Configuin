namespace EditorConfigEditor.Core.EditorConfigParsing;

public class EditorConfigRawValues
{
    public IReadOnlyCollection<EditorConfigRawValue> Rows { get; }

    public EditorConfigRawValues(IReadOnlyCollection<EditorConfigRawValue> rows)
    {
        Rows = rows;
    }
}