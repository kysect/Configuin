using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig.Diff;

public class EditorConfigSettingsComparator
{
    public EditorConfigSettingsDiff Compare(EditorConfigDocument left, EditorConfigDocument right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        IReadOnlyCollection<EditorConfigSettingsRuleSeverityDiff> severityDiffs = GetSeverityDiff(
            left.DescendantNodes().OfType<EditorConfigRuleSeverityNode>().ToList(),
            right.DescendantNodes().OfType<EditorConfigRuleSeverityNode>().ToList());

        IReadOnlyCollection<EditorConfigSettingsRuleOptionDiff> optionDiffs = GetOptionDiff(
            left.DescendantNodes().OfType<EditorConfigRuleOptionNode>().ToList(),
            right.DescendantNodes().OfType<EditorConfigRuleOptionNode>().ToList());

        return new EditorConfigSettingsDiff(severityDiffs, optionDiffs);
    }

    private IReadOnlyCollection<EditorConfigSettingsRuleSeverityDiff> GetSeverityDiff(
        IReadOnlyCollection<EditorConfigRuleSeverityNode> leftRules,
        IReadOnlyCollection<EditorConfigRuleSeverityNode> rightRules)
    {
        var diff = new List<EditorConfigSettingsRuleSeverityDiff>();

        foreach (EditorConfigRuleSeverityNode leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            EditorConfigRuleSeverityNode? rightRule = rightRules.SingleOrDefault(r => r.RuleId.Equals(leftRule.RuleId));
            if (rightRule == null)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(leftRule.RuleId, leftRule.ParseSeverity(), null));
            }
            else if (leftRule.Severity != rightRule.Severity)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(leftRule.RuleId, leftRule.ParseSeverity(), rightRule.ParseSeverity()));
            }
        }

        foreach (EditorConfigRuleSeverityNode rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            EditorConfigRuleSeverityNode? leftRule = leftRules.SingleOrDefault(r => r.RuleId.Equals(rightRule.RuleId));
            if (leftRule is null)
            {
                diff.Add(new EditorConfigSettingsRuleSeverityDiff(rightRule.RuleId, null, rightRule.ParseSeverity()));
            }
        }

        return diff;
    }

    // TODO: reduce copy paste?
    private IReadOnlyCollection<EditorConfigSettingsRuleOptionDiff> GetOptionDiff(
        IReadOnlyCollection<EditorConfigRuleOptionNode> leftRules,
        IReadOnlyCollection<EditorConfigRuleOptionNode> rightRules)
    {
        var diff = new List<EditorConfigSettingsRuleOptionDiff>();

        foreach (EditorConfigRuleOptionNode leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            EditorConfigRuleOptionNode? rightRule = rightRules.SingleOrDefault(r => r.Key.Equals(leftRule.Key));
            if (rightRule == null)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(leftRule.Key, leftRule.Value, null));
            }
            else if (leftRule.Value != rightRule.Value)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(leftRule.Key, leftRule.Value, rightRule.Value));
            }
        }

        foreach (EditorConfigRuleOptionNode rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            EditorConfigRuleOptionNode? leftRule = leftRules.SingleOrDefault(r => r.Key.Equals(rightRule.Key));
            if (leftRule is null)
            {
                diff.Add(new EditorConfigSettingsRuleOptionDiff(rightRule.Key, null, rightRule.Value));
            }
        }

        return diff;
    }
}