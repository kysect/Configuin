using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;

namespace Kysect.Configuin.DotnetConfig.Diff;

public class DotnetConfigDocumentComparator
{
    public DotnetConfigDocumentDiff Compare(DotnetConfigDocument left, DotnetConfigDocument right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        IReadOnlyCollection<DotnetConfigRuleSeverityDiff> severityDiffs = GetSeverityDiff(
            left.DescendantNodes().OfType<DotnetConfigRuleSeverityNode>().ToList(),
            right.DescendantNodes().OfType<DotnetConfigRuleSeverityNode>().ToList());

        IReadOnlyCollection<DotnetConfigRuleOptionDiff> optionDiffs = GetOptionDiff(
            left.DescendantNodes().OfType<DotnetConfigRuleOptionNode>().ToList(),
            right.DescendantNodes().OfType<DotnetConfigRuleOptionNode>().ToList());

        return new DotnetConfigDocumentDiff(severityDiffs, optionDiffs);
    }

    private IReadOnlyCollection<DotnetConfigRuleSeverityDiff> GetSeverityDiff(
        IReadOnlyCollection<DotnetConfigRuleSeverityNode> leftRules,
        IReadOnlyCollection<DotnetConfigRuleSeverityNode> rightRules)
    {
        var diff = new List<DotnetConfigRuleSeverityDiff>();

        foreach (DotnetConfigRuleSeverityNode leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            DotnetConfigRuleSeverityNode? rightRule = rightRules.SingleOrDefault(r => r.RuleId.Equals(leftRule.RuleId));
            if (rightRule == null)
            {
                diff.Add(new DotnetConfigRuleSeverityDiff(leftRule.RuleId, leftRule.ParseSeverity(), null));
            }
            else if (leftRule.Severity != rightRule.Severity)
            {
                diff.Add(new DotnetConfigRuleSeverityDiff(leftRule.RuleId, leftRule.ParseSeverity(), rightRule.ParseSeverity()));
            }
        }

        foreach (DotnetConfigRuleSeverityNode rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            DotnetConfigRuleSeverityNode? leftRule = leftRules.SingleOrDefault(r => r.RuleId.Equals(rightRule.RuleId));
            if (leftRule is null)
            {
                diff.Add(new DotnetConfigRuleSeverityDiff(rightRule.RuleId, null, rightRule.ParseSeverity()));
            }
        }

        return diff;
    }

    // TODO: reduce copy paste?
    private IReadOnlyCollection<DotnetConfigRuleOptionDiff> GetOptionDiff(
        IReadOnlyCollection<DotnetConfigRuleOptionNode> leftRules,
        IReadOnlyCollection<DotnetConfigRuleOptionNode> rightRules)
    {
        var diff = new List<DotnetConfigRuleOptionDiff>();

        foreach (DotnetConfigRuleOptionNode leftRule in leftRules)
        {
            // TODO: enhance message in case when >1 rule
            DotnetConfigRuleOptionNode? rightRule = rightRules.SingleOrDefault(r => r.Key.Equals(leftRule.Key));
            if (rightRule == null)
            {
                diff.Add(new DotnetConfigRuleOptionDiff(leftRule.Key, leftRule.Value, null));
            }
            else if (leftRule.Value != rightRule.Value)
            {
                diff.Add(new DotnetConfigRuleOptionDiff(leftRule.Key, leftRule.Value, rightRule.Value));
            }
        }

        foreach (DotnetConfigRuleOptionNode rightRule in rightRules)
        {
            // TODO: enhance message in case when >1 rule
            DotnetConfigRuleOptionNode? leftRule = leftRules.SingleOrDefault(r => r.Key.Equals(rightRule.Key));
            if (leftRule is null)
            {
                diff.Add(new DotnetConfigRuleOptionDiff(rightRule.Key, null, rightRule.Value));
            }
        }

        return diff;
    }
}