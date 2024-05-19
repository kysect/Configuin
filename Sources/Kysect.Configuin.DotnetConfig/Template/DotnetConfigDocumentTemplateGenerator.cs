using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetConfig.Template;

public class DotnetConfigDocumentTemplateGenerator(ILogger logger)
{
    public string GenerateTemplate(RoslynRules rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        logger.LogInformation("Generating .editorconfig template.");

        var builder = new DotnetConfigDocumentTemplateBuilder();

        foreach (RoslynStyleRuleGroup roslynStyleRuleGroup in rules.StyleRuleGroups)
        {
            foreach (RoslynStyleRule roslynStyleRule in roslynStyleRuleGroup.Rules)
                builder.AddDoubleCommentString($"{roslynStyleRule.Title} ({roslynStyleRule.RuleId})");

            builder.AddDoubleCommentString(roslynStyleRuleGroup.Overview);
            if (roslynStyleRuleGroup.Example is not null)
                builder.AddDoubleCommentString(roslynStyleRuleGroup.Example);

            foreach (RoslynStyleRule roslynStyleRule in roslynStyleRuleGroup.Rules)
                builder.AddCommentString($"dotnet_diagnostic.{roslynStyleRule.RuleId}.severity = ");

            builder.AddEmptyLine();

            if (roslynStyleRuleGroup.Options.Any())
            {
                builder.AddDoubleCommentString("Options:");
                foreach (RoslynStyleRuleOption roslynStyleRuleOption in roslynStyleRuleGroup.Options)
                {
                    builder.AddDoubleCommentString(roslynStyleRuleOption.Name);
                    foreach (RoslynStyleRuleOptionValue roslynStyleRuleOptionValue in roslynStyleRuleOption.Values)
                        builder.AddDoubleCommentString($"- {roslynStyleRuleOptionValue.Value} - {roslynStyleRuleOptionValue.Description}");
                    if (roslynStyleRuleOption.CsharpCodeSample is not null)
                        builder.AddDoubleCommentString(roslynStyleRuleOption.CsharpCodeSample);
                    builder.AddCommentString($"{roslynStyleRuleOption.Name} = ");
                    builder.AddEmptyLine();
                }
            }
        }

        foreach (RoslynQualityRule qualityRule in rules.QualityRules)
        {
            builder.AddDoubleCommentString($"{qualityRule.Title} ({qualityRule.RuleId})");
            builder.AddDoubleCommentString(qualityRule.Description);
            builder.AddCommentString($"dotnet_diagnostic.{qualityRule.RuleId}.severity = ");
            builder.AddEmptyLine();
        }

        return builder.Build();
    }
}