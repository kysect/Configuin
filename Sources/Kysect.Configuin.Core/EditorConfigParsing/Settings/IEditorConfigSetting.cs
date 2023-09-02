using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.EditorConfigParsing.Settings;

public interface IEditorConfigSetting
{
}

public record CompositeRoslynOptionEditorConfigSetting(
    IReadOnlyCollection<string> KeyParts,
    string Value,
    RoslynRuleSeverity? Severity) : IEditorConfigSetting;

public record GeneralEditorConfigSetting(
    string Key,
    string Value) : IEditorConfigSetting;

public record RoslynOptionEditorConfigSetting(
    string Key,
    string Value,
    // TODO: ensure that this is supported (severity per option)
    RoslynRuleSeverity? Severity) : IEditorConfigSetting
{
    public RoslynOptionEditorConfigSetting(string Key, string Value) : this(Key, Value, null)
    {
    }
}

public record RoslynSeverityEditorConfigSetting(
    RoslynRuleId RuleId,
    RoslynRuleSeverity Severity) : IEditorConfigSetting;