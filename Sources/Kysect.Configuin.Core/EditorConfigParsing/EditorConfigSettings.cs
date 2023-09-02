using Kysect.Configuin.Core.EditorConfigParsing.Settings;

namespace Kysect.Configuin.Core.EditorConfigParsing;

public class EditorConfigSettings
{
    public IReadOnlyCollection<IEditorConfigSetting> Settings { get; }

    public EditorConfigSettings(IReadOnlyCollection<IEditorConfigSetting> settings)
    {
        Settings = settings;
    }
}