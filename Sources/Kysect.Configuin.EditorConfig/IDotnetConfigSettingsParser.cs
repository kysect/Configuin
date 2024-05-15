using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig;

public interface IDotnetConfigSettingsParser
{
    DotnetConfigSettings Parse(EditorConfigDocument editorConfigDocument);
}