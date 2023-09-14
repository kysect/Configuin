using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Template;

public class EditorConfigTemplateGenerator
{
    public string GenerateTemplate(RoslynRules rules)
    {
        var builder = new EditorConfigTemplateBuilder();

        foreach (RoslynStyleRule roslynStyleRule in rules.StyleRules)
        {
            builder.AddDoubleCommentString($"{roslynStyleRule.Title} ({roslynStyleRule.RuleId})");
            builder.AddDoubleCommentString(roslynStyleRule.Overview);
            builder.AddCommentString($"dotnet_diagnostic.{roslynStyleRule.RuleId}.severity = ");

            if (roslynStyleRule.Options.Any())
            {
                builder.AddDoubleCommentString($"{roslynStyleRule.RuleId} options:");
                foreach (RoslynStyleRuleOption roslynStyleRuleOption in roslynStyleRule.Options)
                {
                    builder.AddDoubleCommentString(roslynStyleRuleOption.Name);
                    foreach (RoslynStyleRuleOptionValue roslynStyleRuleOptionValue in roslynStyleRuleOption.Values)
                        builder.AddDoubleCommentString($"- {roslynStyleRuleOptionValue.Value} - {roslynStyleRuleOptionValue.Description}");
                    if (roslynStyleRuleOption.CsharpCodeSample is not null)
                        builder.AddDoubleCommentString(roslynStyleRuleOption.CsharpCodeSample);
                    builder.AddCommentString($"{roslynStyleRuleOption.Name} = ");
                }
            }
        }

        foreach (RoslynQualityRule qualityRule in rules.QualityRules)
        {
            builder.AddDoubleCommentString($"{qualityRule.Title} ({qualityRule.RuleId})");
            builder.AddDoubleCommentString(qualityRule.Description);
            builder.AddCommentString($"dotnet_diagnostic.{qualityRule.RuleId}.severity = ");
        }

        return builder.Build();
    }
}