using Kysect.CommonLib.Collections.Extensions;
using Kysect.CommonLib.Logging;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.EditorConfig.Analyzing;

public class EditorConfigAnalyzeLogReporter : IEditorConfigAnalyzeReporter
{
    private readonly ILogger _logger;

    public EditorConfigAnalyzeLogReporter(ILogger logger)
    {
        _logger = logger;
    }

    public void ReportMissedConfigurations(EditorConfigMissedConfiguration editorConfigMissedConfiguration)
    {
        if (editorConfigMissedConfiguration.StyleRuleSeverity.Any())
        {
            _logger.LogInformation("Missed style rules:");
            foreach (RoslynRuleId roslynRuleId in editorConfigMissedConfiguration.StyleRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (editorConfigMissedConfiguration.QualityRuleSeverity.Any())
        {
            _logger.LogInformation("Missed quality rules:");
            foreach (RoslynRuleId roslynRuleId in editorConfigMissedConfiguration.QualityRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (editorConfigMissedConfiguration.StyleRuleOptions.Any())
        {
            _logger.LogInformation("Missed options:");
            foreach (string styleRuleOption in editorConfigMissedConfiguration.StyleRuleOptions)
                _logger.LogTabInformation(1, styleRuleOption);
        }
    }

    public void ReportIncorrectOptionValues(IReadOnlyCollection<EditorConfigInvalidOptionValue> incorrectOptionValues)
    {
        if (incorrectOptionValues.Any())
            _logger.LogInformation("Incorrect option value:");

        foreach (EditorConfigInvalidOptionValue editorConfigInvalidOptionValue in incorrectOptionValues)
        {
            string availableOptions = editorConfigInvalidOptionValue.AvailableOptions.ToSingleString(o => o.Value);
            _logger.LogTabInformation(1, $"Option {editorConfigInvalidOptionValue.Key} has value {editorConfigInvalidOptionValue.Value} but available values: [{availableOptions}]");
        }
    }

    public void ReportIncorrectOptionSeverity(IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity)
    {
        if (incorrectOptionSeverity.Any())
            _logger.LogInformation("Some .editorconfig configuration reference to incorrect rule ids.");

        foreach (RoslynRuleId ruleId in incorrectOptionSeverity)
            _logger.LogTabInformation(1, ruleId.ToString());
    }
}