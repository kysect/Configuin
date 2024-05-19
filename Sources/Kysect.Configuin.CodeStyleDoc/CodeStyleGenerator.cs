using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.Common;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace Kysect.Configuin.CodeStyleDoc;

public class CodeStyleGenerator : ICodeStyleGenerator
{
    private readonly ILogger _logger;

    public CodeStyleGenerator(ILogger logger)
    {
        _logger = logger;
    }

    public CodeStyle Generate(EditorConfigDocument editorConfigDocument, RoslynRules roslynRules)
    {
        editorConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        _logger.LogInformation("Start code style generating.");

        IReadOnlyCollection<RoslynStyleRuleOption> roslynRuleOptions = roslynRules.GetOptions();
        IReadOnlyCollection<IEditorConfigNode> notProcessedSettings = editorConfigDocument.DescendantNodes();

        _logger.LogInformation("Try parse {count} settings", notProcessedSettings.Count);
        notProcessedSettings = notProcessedSettings.Where(IsSupported).ToImmutableList();

        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations = notProcessedSettings
            .OfType<EditorConfigRuleOptionNode>()
            .Select(o => ParseOptionSettings(o, roslynRuleOptions))
            .ToList();
        _logger.LogInformation("Parsed {count} option configurations", optionConfigurations.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not EditorConfigRuleOptionNode).ToImmutableList();

        IReadOnlyCollection<ICodeStyleElement> ruleConfiguration = notProcessedSettings
            .OfType<EditorConfigRuleSeverityNode>()
            .Select(severitySetting => ParseRuleSettings(severitySetting, optionConfigurations, roslynRules))
            .ToList();
        _logger.LogInformation("Parsed {count} rule severity", ruleConfiguration.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not EditorConfigRuleSeverityNode).ToImmutableList();

        if (notProcessedSettings.Any())
        {
            string unsupportedTypes = notProcessedSettings
                .Select(r => r.GetType())
                .Distinct()
                .ToSingleString();

            throw new ConfiguinException($"Rule type is not supported: {unsupportedTypes}");
        }

        return new CodeStyle(ruleConfiguration);
    }

    private bool IsSupported(IEditorConfigNode setting)
    {
        if (setting is not IEditorConfigPropertyNode)
        {
            return false;
        }

        // TODO: #35 support parsing for this rule 
        if (setting is EditorConfigRuleSeverityNode severityEditorConfigRule
            && severityEditorConfigRule.RuleId.Equals(RoslynRuleId.Parse("IDE1006")))
        {
            _logger.LogWarning("Rule IDE1006 is not supported and will be skipped.");
            return false;
        }

        // TODO: #35 Probably, most of this rules related to IDE1006
        if (setting is EditorConfigRuleCompositeOptionNode compositeSetting)
        {
            _logger.LogWarning("{setting} is not supported and will be skipped.", compositeSetting.ToFullString());
            return false;
        }

        // TODO: Maybe we need to support it in some way
        if (setting is EditorConfigGeneralOptionNode generalSetting)
        {
            _logger.LogWarning("{option} is not supported and will be skipped.", generalSetting.ToFullString());
            return false;
        }

        return true;
    }

    private CodeStyleRoslynOptionConfiguration ParseOptionSettings(EditorConfigRuleOptionNode optionEditorConfigSetting, IReadOnlyCollection<RoslynStyleRuleOption> styleRuleOptions)
    {
        RoslynStyleRuleOption? roslynStyleRuleOption = styleRuleOptions.SingleOrDefault(o => o.Name == optionEditorConfigSetting.Key);

        if (roslynStyleRuleOption is null)
            throw new ConfiguinException($"Option {optionEditorConfigSetting.Key} was not found in documentation");

        return new CodeStyleRoslynOptionConfiguration(roslynStyleRuleOption, optionEditorConfigSetting.Value);
    }

    private ICodeStyleElement ParseRuleSettings(
        EditorConfigRuleSeverityNode severityEditorConfigSetting,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        RoslynRules roslynRules)
    {
        RoslynStyleRuleGroup? ruleGroup = roslynRules.StyleRuleGroups.SingleOrDefault(g => g.Rules.Any(r => r.RuleId.Equals(severityEditorConfigSetting.RuleId)));

        if (ruleGroup is not null)
        {
            RoslynStyleRule rule = ruleGroup.Rules.Single(r => r.RuleId.Equals(severityEditorConfigSetting.RuleId));
            var options = ruleGroup
                .Options
                .Select(o => FindOptionConfiguration(optionConfigurations, o.Name))
                .WhereNotNull()
                .ToList();

            return new CodeStyleRoslynStyleRuleConfiguration(rule, severityEditorConfigSetting.ParseSeverity(), options, ruleGroup.Overview, ruleGroup.Example);
        }

        RoslynQualityRule? roslynQualityRule = roslynRules.QualityRules.FirstOrDefault(q => q.RuleId.Equals(severityEditorConfigSetting.RuleId));
        if (roslynQualityRule is not null)
        {
            return new CodeStyleRoslynQualityRuleConfiguration(roslynQualityRule, severityEditorConfigSetting.ParseSeverity());
        }

        throw new ConfiguinException($"Rule with id {severityEditorConfigSetting.RuleId} was not found");
    }

    private CodeStyleRoslynOptionConfiguration? FindOptionConfiguration(
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        string name)
    {
        return optionConfigurations.FirstOrDefault(o => o.Option.Name == name);
    }
}