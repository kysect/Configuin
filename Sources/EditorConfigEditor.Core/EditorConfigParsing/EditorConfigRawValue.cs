namespace EditorConfigEditor.Core.EditorConfigParsing;

public class EditorConfigRawValue
{
    public string RawLine { get; }

    public EditorConfigRawValue(string rawLine)
    {
        RawLine = rawLine;
    }
}