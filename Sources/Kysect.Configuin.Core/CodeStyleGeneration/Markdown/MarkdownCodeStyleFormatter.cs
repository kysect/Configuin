using Kysect.CommonLib.Exceptions;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Markdown;

public class MarkdownCodeStyleFormatter : ICodeStyleFormatter
{
    public string Format(CodeStyle codeStyle)
    {
        var strings = codeStyle.Elements
            .Select(FormatRule)
            .ToList();

        return string.Join(Environment.NewLine, strings);
    }

    public string FormatRule(ICodeStyleElement element)
    {
        return element switch
        {
            CodeStyleRoslynStyleRuleConfiguration styleRule => FormatStyleRule(styleRule),
            CodeStyleRoslynQualityRuleConfiguration qualityRule => FormatQualityRule(qualityRule),
            _ => throw SwitchDefaultException.OnUnexpectedType(nameof(element), element)
        };
    }

    public string FormatStyleRule(CodeStyleRoslynStyleRuleConfiguration rule)
    {
        var builder = new MarkdownStringBuilder();

        builder.AddH2($"{rule.Rule.Title} ({rule.Rule.RuleId})");
        builder.AddEmptyLine();
        builder.AddText($"Severity: {rule.Severity}");
        builder.AddEmptyLine();
        builder.AddText(rule.Rule.Overview);
        builder.AddEmptyLine();

        foreach (CodeStyleRoslynOptionConfiguration optionConfiguration in rule.Options)
        {
            builder.AddH3($"{optionConfiguration.Option.Name} = {optionConfiguration.SelectedValue}");
            builder.AddEmptyLine();
            if (optionConfiguration.Option.CsharpCodeSample is not null)
                builder.AddCode(optionConfiguration.Option.CsharpCodeSample);
        }

        return builder.Build();
    }

    public string FormatQualityRule(CodeStyleRoslynQualityRuleConfiguration rule)
    {
        var builder = new MarkdownStringBuilder();

        builder.AddH2($"{rule.Rule.Title} ({rule.Rule.RuleId})");
        builder.AddEmptyLine();
        builder.AddText($"Severity: {rule.Severity}");

        return builder.Build();
    }
}