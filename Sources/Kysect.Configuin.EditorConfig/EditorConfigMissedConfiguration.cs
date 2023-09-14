using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig;

public record EditorConfigMissedConfiguration(
    IReadOnlyCollection<RoslynRuleId> StyleRuleSeverity,
    IReadOnlyCollection<RoslynRuleId> QualityRuleSeverity,
    IReadOnlyCollection<string> StyleRuleOptions
    );


public record EditorConfigInvalidOptionValue(
    string Key,
    string Value,
    IReadOnlyCollection<RoslynStyleRuleOptionValue> AvailableOptions
    );