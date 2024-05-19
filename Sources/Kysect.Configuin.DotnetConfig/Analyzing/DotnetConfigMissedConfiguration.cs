using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.DotnetConfig.Analyzing;

public record DotnetConfigMissedConfiguration(
    IReadOnlyCollection<RoslynRuleId> StyleRuleSeverity,
    IReadOnlyCollection<RoslynRuleId> QualityRuleSeverity,
    IReadOnlyCollection<string> StyleRuleOptions
    );


public record DotnetConfigInvalidOptionValue(
    string Key,
    string Value,
    IReadOnlyCollection<RoslynStyleRuleOptionValue> AvailableOptions
    );