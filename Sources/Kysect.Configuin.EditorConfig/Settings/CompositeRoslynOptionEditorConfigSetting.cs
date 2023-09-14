using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Settings;

public record CompositeRoslynOptionEditorConfigSetting(
    IReadOnlyCollection<string> KeyParts,
    string Value,
    RoslynRuleSeverity? Severity) : IEditorConfigSetting
{
    public string ToDisplayString()
    {
        return $"{string.Join('.', KeyParts)} = {Value} (Severity: {Severity})";
    }
}