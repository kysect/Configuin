using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.DotnetConfig.Analyzing;

public class DotnetConfigDocumentAnalyzer
{
    public DotnetConfigMissedConfiguration GetMissedConfigurations(DotnetConfigDocument dotnetConfigDocument, RoslynRules roslynRules)
    {
        dotnetConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var selectedSeverity = dotnetConfigDocument
            .DescendantNodes()
            .OfType<DotnetConfigRuleSeverityNode>()
            .Select(c => c.RuleId)
            .ToHashSet();

        var selectedOptions = dotnetConfigDocument
            .DescendantNodes()
            .OfType<DotnetConfigRuleOptionNode>()
            .Select(c => c.Key)
            .ToHashSet();

        var missedStyleRules = roslynRules
            .GetStyleRules()
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

        return new DotnetConfigMissedConfiguration(missedStyleRules, missedQualityRules, missedOptions);
    }

    public IReadOnlyCollection<DotnetConfigInvalidOptionValue> GetIncorrectOptionValues(DotnetConfigDocument dotnetConfigDocument, RoslynRules roslynRules)
    {
        dotnetConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var result = new List<DotnetConfigInvalidOptionValue>();

        var optionAvailableValues = roslynRules.GetOptions().ToDictionary(o => o.Name, o => o.Values);

        foreach (var node in dotnetConfigDocument.DescendantNodes().OfType<DotnetConfigRuleOptionNode>())
        {
            string key = node.Key;
            string value = node.Value;
            if (!optionAvailableValues.TryGetValue(key, out IReadOnlyCollection<RoslynStyleRuleOptionValue>? values))
                values = Array.Empty<RoslynStyleRuleOptionValue>();

            if (!values.Any(v => v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
                result.Add(new DotnetConfigInvalidOptionValue(key, value, values));
        }

        return result;
    }

    public IReadOnlyCollection<RoslynRuleId> GetIncorrectOptionSeverity(DotnetConfigDocument dotnetConfigDocument, RoslynRules roslynRules)
    {
        dotnetConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var ruleIds = new HashSet<RoslynRuleId>();
        ruleIds.AddEach(roslynRules.GetStyleRules().Select(r => r.RuleId));
        ruleIds.AddEach(roslynRules.QualityRules.Select(r => r.RuleId));

        var result = new List<RoslynRuleId>();
        foreach (DotnetConfigRuleSeverityNode value in dotnetConfigDocument.DescendantNodes().OfType<DotnetConfigRuleSeverityNode>())
        {
            if (!ruleIds.Contains(value.RuleId))
                result.Add(value.RuleId);
        }

        return result;
    }
}