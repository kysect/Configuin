using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.Common;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.CodeStyleDoc;

public class CodeStyleGenerator : ICodeStyleGenerator
{
    private readonly ILogger _logger;

    public CodeStyleGenerator(ILogger logger)
    {
        _logger = logger;
    }

    public CodeStyle Generate(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules)
    {
        _logger.LogInformation("Start code style generating.");

        IReadOnlyCollection<RoslynStyleRuleOption> roslynRuleOptions = roslynRules.GetOptions();
        IReadOnlyCollection<IEditorConfigSetting> notProcessedSettings = editorConfigSettings.Settings;

        _logger.LogInformation("Try parse {count} settings", notProcessedSettings.Count);
        notProcessedSettings = notProcessedSettings.Where(IsSupported).ToList();

        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations = notProcessedSettings
            .OfType<RoslynOptionEditorConfigSetting>()
            .Select(o => ParseOptionSettings(o, roslynRuleOptions))
            .ToList();
        _logger.LogInformation("Parsed {count} option configurations", optionConfigurations.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not RoslynOptionEditorConfigSetting).ToList();

        IReadOnlyCollection<ICodeStyleElement> ruleConfiguration = notProcessedSettings
            .OfType<RoslynSeverityEditorConfigSetting>()
            .Select(severitySetting => ParseRuleSettings(severitySetting, optionConfigurations, roslynRules))
            .ToList();
        _logger.LogInformation("Parsed {count} rule severity", ruleConfiguration.Count);

        notProcessedSettings = notProcessedSettings.Where(r => r is not RoslynSeverityEditorConfigSetting).ToList();

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

    private bool IsSupported(IEditorConfigSetting setting)
    {
        // TODO: #35 support parsing for this rule 
        if (setting is RoslynSeverityEditorConfigSetting severityEditorConfigRule
            && severityEditorConfigRule.RuleId.Equals(RoslynRuleId.Parse("IDE1006")))
        {
            _logger.LogWarning("Rule IDE0055 is not supported and will be skipped.");
            return false;
        }

        // TODO: #35 Probably, most of this rules related to IDE1006
        if (setting is CompositeRoslynOptionEditorConfigSetting compositeSetting)
        {
            _logger.LogWarning("{setting} is not supported and will be skipped.", compositeSetting.ToDisplayString());
            return false;
        }

        // TODO: Maybe we need to support it in some way
        if (setting is GeneralEditorConfigSetting generalSetting)
        {
            _logger.LogWarning("{option} is not supported and will be skipped.", generalSetting.ToDisplayString());
            return false;
        }

        return true;
    }

    private CodeStyleRoslynOptionConfiguration ParseOptionSettings(RoslynOptionEditorConfigSetting optionEditorConfigSetting, IReadOnlyCollection<RoslynStyleRuleOption> styleRuleOptions)
    {
        RoslynStyleRuleOption? roslynStyleRuleOption = styleRuleOptions.SingleOrDefault(o => o.Name == optionEditorConfigSetting.Key);

        if (roslynStyleRuleOption is null)
            throw new ConfiguinException($"Option {optionEditorConfigSetting.Key} was not found in documentation");

        return new CodeStyleRoslynOptionConfiguration(roslynStyleRuleOption, optionEditorConfigSetting.Value);
    }

    private ICodeStyleElement ParseRuleSettings(
        RoslynSeverityEditorConfigSetting severityEditorConfigSetting,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        RoslynRules roslynRules)
    {
        RoslynStyleRuleGroup? ruleGroup = roslynRules.StyleRuleGroups.SingleOrDefault(g => g.Rules.Any(r => r.RuleId.Equals(severityEditorConfigSetting.RuleId)));

        if (ruleGroup is not null)
        {
            RoslynStyleRule rule = ruleGroup.Rules.Single(r => r.RuleId.Equals(severityEditorConfigSetting.RuleId));
            var options = ruleGroup
                .Options
                .Select(o => GetOptionConfiguration(optionConfigurations, o.Name))
                .ToList();

            return new CodeStyleRoslynStyleRuleConfiguration(rule, severityEditorConfigSetting.Severity, options);
        }

        RoslynQualityRule? roslynQualityRule = roslynRules.QualityRules.FirstOrDefault(q => q.RuleId.Equals(severityEditorConfigSetting.RuleId));
        if (roslynQualityRule is not null)
        {
            return new CodeStyleRoslynQualityRuleConfiguration(roslynQualityRule, severityEditorConfigSetting.Severity);
        }

        throw new ConfiguinException($"Rule with id {severityEditorConfigSetting.RuleId} was not found");
    }

    private CodeStyleRoslynOptionConfiguration GetOptionConfiguration(
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> optionConfigurations,
        string name)
    {
        CodeStyleRoslynOptionConfiguration? option = optionConfigurations.FirstOrDefault(o => o.Option.Name == name);
        if (option is null)
            throw new ConfiguinException($"Option with name {name} was not found");

        return option;
    }
}