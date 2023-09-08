using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Core.EditorConfigParsing.Settings;

public interface IEditorConfigSetting
{
}

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

public record GeneralEditorConfigSetting(
    string Key,
    string Value) : IEditorConfigSetting
{
    public string ToDisplayString()
    {
        return $"{Key} = {Value}";
    }
}

public record RoslynOptionEditorConfigSetting(
    string Key,
    string Value) : IEditorConfigSetting;

public record RoslynSeverityEditorConfigSetting(
    RoslynRuleId RuleId,
    RoslynRuleSeverity Severity) : IEditorConfigSetting;