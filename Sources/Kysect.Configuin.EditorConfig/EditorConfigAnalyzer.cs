using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig;

public class EditorConfigAnalyzer
{
    public EditorConfigMissedConfiguration GetMissedConfigurations(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules)
    {
        var selectedSeverity = editorConfigSettings
            .Settings
            .OfType<RoslynSeverityEditorConfigSetting>()
            .Select(c => c.RuleId)
            .ToHashSet();

        var selectedOptions = editorConfigSettings
            .Settings
            .OfType<RoslynOptionEditorConfigSetting>()
            .Select(c => c.Key)
            .ToHashSet();

        var missedStyleRules = roslynRules
            .StyleRules
            .Where(rule => !selectedSeverity.Contains(rule.RuleId))
            .Select(r => r.RuleId)
            .Order()
            .ToList();

        var missedQualityRules = roslynRules
            .QualityRules
            .Where(rule => !selectedSeverity.Contains(rule.RuleId))
            .Select(r => r.RuleId)
            .Order()
            .ToList();

        var missedOptions = roslynRules
            .GetOptions()
            .Where(option => !selectedOptions.Contains(option.Name))
            .Select(o => o.Name)
            .ToList();

        return new EditorConfigMissedConfiguration(missedStyleRules, missedQualityRules, missedOptions);
    }
}