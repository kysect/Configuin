using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.CommonLib.Logging;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetConfig.Analyzing;

public class DotnetConfigAnalyzeLogReporter : IDotnetConfigAnalyzeReporter
{
    private readonly ILogger _logger;

    public DotnetConfigAnalyzeLogReporter(ILogger logger)
    {
        _logger = logger;
    }

    public void ReportMissedConfigurations(DotnetConfigMissedConfiguration dotnetConfigMissedConfiguration)
    {
        dotnetConfigMissedConfiguration.ThrowIfNull();

        if (dotnetConfigMissedConfiguration.StyleRuleSeverity.Any())
        {
            _logger.LogInformation("Missed style rules:");
            foreach (RoslynRuleId roslynRuleId in dotnetConfigMissedConfiguration.StyleRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (dotnetConfigMissedConfiguration.QualityRuleSeverity.Any())
        {
            _logger.LogInformation("Missed quality rules:");
            foreach (RoslynRuleId roslynRuleId in dotnetConfigMissedConfiguration.QualityRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (dotnetConfigMissedConfiguration.StyleRuleOptions.Any())
        {
            _logger.LogInformation("Missed options:");
            foreach (string styleRuleOption in dotnetConfigMissedConfiguration.StyleRuleOptions)
                _logger.LogTabInformation(1, styleRuleOption);
        }
    }

    public void ReportIncorrectOptionValues(IReadOnlyCollection<DotnetConfigInvalidOptionValue> incorrectOptionValues)
    {
        ArgumentNullException.ThrowIfNull(incorrectOptionValues);

        if (incorrectOptionValues.Any())
            _logger.LogInformation("Incorrect option value:");

        foreach (DotnetConfigInvalidOptionValue editorConfigInvalidOptionValue in incorrectOptionValues)
        {
            string availableOptions = editorConfigInvalidOptionValue.AvailableOptions.ToSingleString(o => o.Value);
            _logger.LogTabInformation(1, $"Option {editorConfigInvalidOptionValue.Key} has value {editorConfigInvalidOptionValue.Value} but available values: [{availableOptions}]");
        }
    }

    public void ReportIncorrectOptionSeverity(IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity)
    {
        ArgumentNullException.ThrowIfNull(incorrectOptionSeverity);

        if (incorrectOptionSeverity.Any())
            _logger.LogInformation("Some .editorconfig configuration reference to incorrect rule ids.");

        foreach (RoslynRuleId ruleId in incorrectOptionSeverity)
            _logger.LogTabInformation(1, ruleId.ToString());
    }
}