using Kysect.CommonLib.Collections.Extensions;
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

    public IReadOnlyCollection<EditorConfigInvalidOptionValue> GetIncorrectOptionValues(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules)
    {
        var result = new List<EditorConfigInvalidOptionValue>();

        var optionAvailableValues = roslynRules.GetOptions().ToDictionary(o => o.Name, o => o.Values);

        foreach ((string key, string value) in editorConfigSettings.Settings.OfType<RoslynOptionEditorConfigSetting>())
        {
            if (!optionAvailableValues.TryGetValue(key, out IReadOnlyCollection<RoslynStyleRuleOptionValue>? values))
                values = Array.Empty<RoslynStyleRuleOptionValue>();

            if (!values.Any(v => v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
                result.Add(new EditorConfigInvalidOptionValue(key, value, values));
        }

        return result;
    }

    public IReadOnlyCollection<RoslynRuleId> GetIncorrectOptionSeverity(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules)
    {
        var ruleIds = new HashSet<RoslynRuleId>();
        ruleIds.AddEach(roslynRules.StyleRules.Select(r => r.RuleId));
        ruleIds.AddEach(roslynRules.QualityRules.Select(r => r.RuleId));

        var result = new List<RoslynRuleId>();
        foreach ((RoslynRuleId roslynRuleId, RoslynRuleSeverity _) in editorConfigSettings.Settings.OfType<RoslynSeverityEditorConfigSetting>())
        {
            if (!ruleIds.Contains(roslynRuleId))
                result.Add(roslynRuleId);
        }    

        return result;
    }
}