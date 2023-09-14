using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Settings;

public record RoslynSeverityEditorConfigSetting(
    RoslynRuleId RuleId,
    RoslynRuleSeverity Severity) : IEditorConfigSetting;