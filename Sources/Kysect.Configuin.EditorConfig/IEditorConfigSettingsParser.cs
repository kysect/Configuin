namespace Kysect.Configuin.EditorConfig;

public interface IEditorConfigSettingsParser
{
    EditorConfigSettings Parse(string content);
}