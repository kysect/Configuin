using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.DotnetConfig.Diff;

public record DotnetConfigRuleSeverityDiff(RoslynRuleId Id, RoslynRuleSeverity? Left, RoslynRuleSeverity? Right);
public record DotnetConfigRuleOptionDiff(string Key, string? Left, string? Right);
public record DotnetConfigDocumentDiff(IReadOnlyCollection<DotnetConfigRuleSeverityDiff> SeverityDiffs, IReadOnlyCollection<DotnetConfigRuleOptionDiff> OptionDiffs);