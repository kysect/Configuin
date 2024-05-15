using Kysect.Configuin.EditorConfig.Settings;

namespace Kysect.Configuin.EditorConfig;

public class DotnetConfigSettings
{
    public IReadOnlyCollection<IEditorConfigSetting> Settings { get; }

    public DotnetConfigSettings(IReadOnlyCollection<IEditorConfigSetting> settings)
    {
        Settings = settings;
    }
}