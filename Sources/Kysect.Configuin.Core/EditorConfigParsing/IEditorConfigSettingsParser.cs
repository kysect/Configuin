namespace Kysect.Configuin.Core.EditorConfigParsing;

public interface IEditorConfigSettingsParser
{
    EditorConfigSettings Parse(string content);
}