using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Analyzing;

public class EditorConfigAnalyzer
{
    public EditorConfigMissedConfiguration GetMissedConfigurations(EditorConfigDocument editorConfigDocument, RoslynRules roslynRules)
    {
        editorConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var selectedSeverity = editorConfigDocument
            .DescendantNodes()
            .OfType<EditorConfigRuleSeverityNode>()
            .Select(c => c.RuleId)
            .ToHashSet();

        var selectedOptions = editorConfigDocument
            .DescendantNodes()
            .OfType<EditorConfigRuleOptionNode>()
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

        return new EditorConfigMissedConfiguration(missedStyleRules, missedQualityRules, missedOptions);
    }

    public IReadOnlyCollection<EditorConfigInvalidOptionValue> GetIncorrectOptionValues(EditorConfigDocument editorConfigDocument, RoslynRules roslynRules)
    {
        editorConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var result = new List<EditorConfigInvalidOptionValue>();

        var optionAvailableValues = roslynRules.GetOptions().ToDictionary(o => o.Name, o => o.Values);

        foreach (var node in editorConfigDocument.DescendantNodes().OfType<EditorConfigRuleOptionNode>())
        {
            string key = node.Key;
            string value = node.Value;
            if (!optionAvailableValues.TryGetValue(key, out IReadOnlyCollection<RoslynStyleRuleOptionValue>? values))
                values = Array.Empty<RoslynStyleRuleOptionValue>();

            if (!values.Any(v => v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
                result.Add(new EditorConfigInvalidOptionValue(key, value, values));
        }

        return result;
    }

    public IReadOnlyCollection<RoslynRuleId> GetIncorrectOptionSeverity(EditorConfigDocument editorConfigDocument, RoslynRules roslynRules)
    {
        editorConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        var ruleIds = new HashSet<RoslynRuleId>();
        ruleIds.AddEach(roslynRules.GetStyleRules().Select(r => r.RuleId));
        ruleIds.AddEach(roslynRules.QualityRules.Select(r => r.RuleId));

        var result = new List<RoslynRuleId>();
        foreach (EditorConfigRuleSeverityNode value in editorConfigDocument.DescendantNodes().OfType<EditorConfigRuleSeverityNode>())
        {
            if (!ruleIds.Contains(value.RuleId))
                result.Add(value.RuleId);
        }

        return result;
    }
}