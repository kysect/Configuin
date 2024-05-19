using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.DotnetConfig.Analyzing;

public interface IDotnetConfigAnalyzeReporter
{
    void ReportMissedConfigurations(DotnetConfigMissedConfiguration dotnetConfigMissedConfiguration);
    void ReportIncorrectOptionValues(IReadOnlyCollection<DotnetConfigInvalidOptionValue> incorrectOptionValues);
    void ReportIncorrectOptionSeverity(IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity);
}