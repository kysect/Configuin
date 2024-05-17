using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Settings;

namespace Kysect.Configuin.EditorConfig;

public interface IDotnetConfigSettingsParser
{
    DotnetConfigSettings Parse(EditorConfigDocument editorConfigDocument);
    IEditorConfigSetting ParseSetting(EditorConfigPropertyNode line);
}