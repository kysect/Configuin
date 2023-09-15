namespace Kysect.Configuin.EditorConfig.Settings;

public record GeneralEditorConfigSetting(
    string Key,
    string Value) : IEditorConfigSetting
{
    public string ToDisplayString()
    {
        return $"{Key} = {Value}";
    }
}