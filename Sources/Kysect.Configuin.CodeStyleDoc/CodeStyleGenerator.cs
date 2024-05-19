using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.Common;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
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

    public CodeStyle Generate(DotnetConfigDocument dotnetConfigDocument, RoslynRules roslynRules)
    {
        dotnetConfigDocument.ThrowIfNull();
        roslynRules.ThrowIfNull();

        _logger.LogInformation("Start code style generating.");

        IReadOnlyCollection<RoslynStyleRuleOption> roslynRuleOptions = roslynRules.GetOptions();
        IReadOnlyCollection<IDotnetConfigSyntaxNode> notProcessedSettings = dotnetConfigDocument.DescendantNodes();

        _logger.LogInformation("Try parse {count} settings", notProcessedSettings.Count);
        notProcessedSettings = notProcessedSettings.Where(IsSupported).ToImmutableList();

        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations = notProcessedSettings
            .OfType<DotnetConfigRuleOptionNode>()
            .Select(o => ParseOptionSettings(o, roslynRuleOptions))
            .ToList();
        _logger.LogInformation("Parsed {count} option configurations", optionConfigurations.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not DotnetConfigRuleOptionNode).ToImmutableList();

        IReadOnlyCollection<ICodeStyleElement> ruleConfiguration = notProcessedSettings
            .OfType<DotnetConfigRuleSeverityNode>()
            .Select(severitySetting => ParseRuleSettings(severitySetting, optionConfigurations, roslynRules))
            .ToList();
        _logger.LogInformation("Parsed {count} rule severity", ruleConfiguration.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not DotnetConfigRuleSeverityNode).ToImmutableList();

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

    private bool IsSupported(IDotnetConfigSyntaxNode setting)
    {
        if (setting is not IDotnetConfigPropertySyntaxNode)
        {
            return false;
        }

        // TODO: #35 support parsing for this rule 
        if (setting is DotnetConfigRuleSeverityNode severityEditorConfigRule
            && severityEditorConfigRule.RuleId.Equals(RoslynRuleId.Parse("IDE1006")))
        {
            _logger.LogWarning("Rule IDE1006 is not supported and will be skipped.");
            return false;
        }

        // TODO: #35 Probably, most of this rules related to IDE1006
        if (setting is DotnetConfigRuleCompositeOptionNode compositeSetting)
        {
            _logger.LogWarning("{setting} is not supported and will be skipped.", compositeSetting.ToFullString());
            return false;
        }

        // TODO: Maybe we need to support it in some way
        if (setting is DotnetConfigGeneralOptionNode generalSetting)
        {
            _logger.LogWarning("{option} is not supported and will be skipped.", generalSetting.ToFullString());
            return false;
        }

        return true;
    }

    private CodeStyleRoslynOptionConfiguration ParseOptionSettings(DotnetConfigRuleOptionNode optionDotnetConfigSetting, IReadOnlyCollection<RoslynStyleRuleOption> styleRuleOptions)
    {
        RoslynStyleRuleOption? roslynStyleRuleOption = styleRuleOptions.SingleOrDefault(o => o.Name == optionDotnetConfigSetting.Key);

        if (roslynStyleRuleOption is null)
            throw new ConfiguinException($"Option {optionDotnetConfigSetting.Key} was not found in documentation");

        return new CodeStyleRoslynOptionConfiguration(roslynStyleRuleOption, optionDotnetConfigSetting.Value);
    }

    private ICodeStyleElement ParseRuleSettings(
        DotnetConfigRuleSeverityNode severityDotnetConfigSetting,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        RoslynRules roslynRules)
    {
        RoslynStyleRuleGroup? ruleGroup = roslynRules.StyleRuleGroups.SingleOrDefault(g => g.Rules.Any(r => r.RuleId.Equals(severityDotnetConfigSetting.RuleId)));

        if (ruleGroup is not null)
        {
            RoslynStyleRule rule = ruleGroup.Rules.Single(r => r.RuleId.Equals(severityDotnetConfigSetting.RuleId));
            var options = ruleGroup
                .Options
                .Select(o => FindOptionConfiguration(optionConfigurations, o.Name))
                .WhereNotNull()
                .ToList();

            return new CodeStyleRoslynStyleRuleConfiguration(rule, severityDotnetConfigSetting.ParseSeverity(), options, ruleGroup.Overview, ruleGroup.Example);
        }

        RoslynQualityRule? roslynQualityRule = roslynRules.QualityRules.FirstOrDefault(q => q.RuleId.Equals(severityDotnetConfigSetting.RuleId));
        if (roslynQualityRule is not null)
        {
            return new CodeStyleRoslynQualityRuleConfiguration(roslynQualityRule, severityDotnetConfigSetting.ParseSeverity());
        }

        throw new ConfiguinException($"Rule with id {severityDotnetConfigSetting.RuleId} was not found");
    }

    private CodeStyleRoslynOptionConfiguration? FindOptionConfiguration(
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        string name)
    {
        return optionConfigurations.FirstOrDefault(o => o.Option.Name == name);
    }
}