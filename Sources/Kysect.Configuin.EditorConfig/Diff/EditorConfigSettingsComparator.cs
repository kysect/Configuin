using Kysect.Configuin.EditorConfig.Settings;

namespace Kysect.Configuin.EditorConfig.Diff;

public class EditorConfigSettingsComparator
{
    public EditorConfigSettingsDiff Compare(EditorConfigSettings left, EditorConfigSettings right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        IReadOnlyCollection<EditorConfigSettingsRuleSeverityDiff> severityDiffs = GetSeverityDiff(
            left.Settings.OfType<RoslynSeverityEditorConfigSetting>().ToList(),
            right.Settings.OfType<RoslynSeverityEditorConfigSetting>().ToList());

        IReadOnlyCollection<EditorConfigSettingsRuleOptionDiff> optionDiffs = GetOptionDiff(
            left.Settings.OfType<RoslynOptionEditorConfigSetting>().ToList(),
            right.Settings.OfType<RoslynOptionEditorConfigSetting>().ToList());

        return new EditorConfigSettingsDiff(severityDiffs, optionDiffs);
    }

    private IReadOnlyCollection<EditorConfigSettingsRuleSeverityDiff> GetSeverityDiff(
        IReadOnlyCollection<RoslynSeverityEditorConfigSetting> leftRules,
        IReadOnlyCollection<RoslynSeverityEditorConfigSetting> rightRules)
    {
        var diff = new List<EditorConfigSettingsRuleSeverityDiff>();

        foreach (RoslynSeverityEditorConfigSetting leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            RoslynSeverityEditorConfigSetting? rightRule = rightRules.SingleOrDefault(r => r.RuleId.Equals(leftRule.RuleId));
            if (rightRule == null)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(leftRule.RuleId, leftRule.Severity, null));
            }
            else if (leftRule.Severity != rightRule.Severity)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(leftRule.RuleId, leftRule.Severity, rightRule.Severity));
            }
        }

        foreach (RoslynSeverityEditorConfigSetting rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            RoslynSeverityEditorConfigSetting? leftRule = leftRules.SingleOrDefault(r => r.RuleId.Equals(rightRule.RuleId));
            if (leftRule is null)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(rightRule.RuleId, null, rightRule.Severity));
            }
        }

        return diff;
    }

    // TODO: reduce copy paste?
    private IReadOnlyCollection<EditorConfigSettingsRuleOptionDiff> GetOptionDiff(
        IReadOnlyCollection<RoslynOptionEditorConfigSetting> leftRules,
        IReadOnlyCollection<RoslynOptionEditorConfigSetting> rightRules)
    {
        var diff = new List<EditorConfigSettingsRuleOptionDiff>();

        foreach (RoslynOptionEditorConfigSetting leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            RoslynOptionEditorConfigSetting? rightRule = rightRules.SingleOrDefault(r => r.Key.Equals(leftRule.Key));
            if (rightRule == null)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(leftRule.Key, leftRule.Value, null));
            }
            else if (leftRule.Value != rightRule.Value)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(leftRule.Key, leftRule.Value, rightRule.Value));
            }
        }

        foreach (RoslynOptionEditorConfigSetting rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            RoslynOptionEditorConfigSetting? leftRule = leftRules.SingleOrDefault(r => r.Key.Equals(rightRule.Key));
            if (leftRule is null)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(rightRule.Key, null, rightRule.Value));
            }
        }

        return diff;
    }
}