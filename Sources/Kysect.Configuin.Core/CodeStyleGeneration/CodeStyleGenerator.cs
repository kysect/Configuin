using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.EditorConfigParsing.Rules;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration;

public class CodeStyleGenerator : ICodeStyleGenerator
{
    public CodeStyleInfo Generate(EditorConfigRuleSet editorConfigRuleSet, RoslynRules roslynRules)
    {
        IReadOnlyCollection<IEditorConfigRule> notProcessedRules = editorConfigRuleSet.Rules;
        IReadOnlyCollection<RoslynStyleRuleOption> optionsFromDocs = roslynRules.GetOptions();

        // TODO: support in some way
        notProcessedRules = notProcessedRules.Where(r => r is not GeneralEditorConfigRule).ToList();

        // TODO: support in some way
        notProcessedRules = notProcessedRules.Where(r => r is not CompositeRoslynOptionEditorConfigRule).ToList();

        IReadOnlyCollection<RoslynOptionConfiguration> roslynOptionEditorConfigRules = notProcessedRules
            .OfType<RoslynOptionEditorConfigRule>()
            .Select(o => ParseOption(o, optionsFromDocs))
            .ToList();

        notProcessedRules = notProcessedRules.Where(r => r is not RoslynOptionEditorConfigRule).ToList();

        IReadOnlyCollection<ICodeStyleElement> elements = notProcessedRules
            .OfType<RoslynSeverityEditorConfigRule>()
            .Select(r => ParseRule(r, roslynOptionEditorConfigRules, roslynRules))
            .ToList();
        notProcessedRules = notProcessedRules.Where(r => r is not RoslynSeverityEditorConfigRule).ToList();

        if (notProcessedRules.Any())
        {
            string unsupportedTypes = notProcessedRules
                .Select(r => r.GetType())
                .Distinct()
                .ToSingleString();

            throw new ConfiguinException($"Rule type is not supported: {unsupportedTypes}");
        }

        return new CodeStyleInfo(elements);
    }

    // TODO: Rework naming
    private RoslynOptionConfiguration ParseOption(RoslynOptionEditorConfigRule optionEditorConfigRule, IReadOnlyCollection<RoslynStyleRuleOption> optionsFromDocs)
    {
        // TODO: check for duplicate?
        RoslynStyleRuleOption? roslynStyleRuleOption = optionsFromDocs.FirstOrDefault(o => o.Name == optionEditorConfigRule.Key);
        if (roslynStyleRuleOption is null)
            throw new ConfiguinException($"Option {optionEditorConfigRule.Key} was not found in documentation");

        return new RoslynOptionConfiguration(roslynStyleRuleOption, optionEditorConfigRule.Value);
    }

    private ICodeStyleElement ParseRule(
        RoslynSeverityEditorConfigRule severityEditorConfigRule,
        IReadOnlyCollection<RoslynOptionConfiguration> roslynOptionEditorConfigRules,
        RoslynRules roslynRules)
    {
        RoslynStyleRule? roslynStyleRule = roslynRules.StyleRules.FirstOrDefault(s => s.RuleId.Equals(severityEditorConfigRule.RuleId));
        if (roslynStyleRule is not null)
        {
            var options = roslynStyleRule
                .Options
                .Select(o => GetOption(roslynOptionEditorConfigRules, o.Name))
                .ToList();

            return new RoslynStyleRuleConfiguration(roslynStyleRule, severityEditorConfigRule.Severity, options);
        }

        RoslynQualityRule? roslynQualityRule = roslynRules.QualityRules.FirstOrDefault(q => q.RuleId.Equals(severityEditorConfigRule.RuleId));
        if (roslynQualityRule is not null)
        {
            return new RoslynQualityRuleConfiguration(roslynQualityRule, severityEditorConfigRule.Severity);
        }

        throw new ConfiguinException($"Rule with id {severityEditorConfigRule.RuleId} was not found");
    }

    private RoslynOptionConfiguration GetOption(
        IReadOnlyCollection<RoslynOptionConfiguration> roslynOptionEditorConfigRules,
        string name)
    {
        RoslynOptionConfiguration? option = roslynOptionEditorConfigRules.FirstOrDefault(o => o.Option.Name == name);
        if (option is null)
            throw new ConfiguinException($"Option with name {name} was not found");

        return option;
    }
}