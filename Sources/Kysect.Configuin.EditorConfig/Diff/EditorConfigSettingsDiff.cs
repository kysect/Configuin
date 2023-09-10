using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Diff;

public record EditorConfigSettingsRuleSeverityDiff(RoslynRuleId Id, RoslynRuleSeverity? Left, RoslynRuleSeverity? Right);
public record EditorConfigSettingsRuleOptionDiff(string Key, string? Left, string? Right);
public record EditorConfigSettingsDiff(IReadOnlyCollection<EditorConfigSettingsRuleSeverityDiff> SeverityDiffs, IReadOnlyCollection<EditorConfigSettingsRuleOptionDiff> OptionDiffs);