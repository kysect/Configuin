namespace Kysect.Configuin.EditorConfig.Settings;

public record RoslynOptionEditorConfigSetting(
    string Key,
    string Value) : IEditorConfigSetting;