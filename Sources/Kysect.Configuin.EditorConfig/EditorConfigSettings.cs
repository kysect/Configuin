using Kysect.Configuin.EditorConfig.Settings;

namespace Kysect.Configuin.EditorConfig;

public class EditorConfigSettings
{
    public IReadOnlyCollection<IEditorConfigSetting> Settings { get; }

    public EditorConfigSettings(IReadOnlyCollection<IEditorConfigSetting> settings)
    {
        Settings = settings;
    }
}