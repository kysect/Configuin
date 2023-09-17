using Kysect.CommonLib.Exceptions;
using Kysect.Configuin.CodeStyleDoc.Models;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.CodeStyleDoc.Markdown;

public class MarkdownCodeStyleFormatter : ICodeStyleFormatter
{
    private readonly ILogger _logger;

    public MarkdownCodeStyleFormatter(ILogger logger)
    {
        _logger = logger;
    }

    public string Format(CodeStyle codeStyle)
    {
        ArgumentNullException.ThrowIfNull(codeStyle);

        _logger.LogInformation("Formatting code style to markdown.");

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
            _ => throw SwitchDefaultExceptions.OnUnexpectedType(nameof(element), element)
        };
    }

    public string FormatStyleRule(CodeStyleRoslynStyleRuleConfiguration rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var builder = new MarkdownStringBuilder();

        builder.AddH2($"{rule.Rule.Title} ({rule.Rule.RuleId})");
        builder.AddEmptyLine();
        builder.AddText($"Severity: {rule.Severity}");
        builder.AddEmptyLine();
        builder.AddText(rule.Overview);
        if (rule.Example is not null)
        {
            builder.AddEmptyLine();
            builder.AddCode(rule.Example);
        }

        foreach (CodeStyleRoslynOptionConfiguration optionConfiguration in rule.Options)
        {
            builder.AddEmptyLine();
            builder.AddH3($"{optionConfiguration.Option.Name} = {optionConfiguration.SelectedValue}");
            if (optionConfiguration.Option.CsharpCodeSample is not null)
            {
                builder.AddEmptyLine();
                builder.AddCode(optionConfiguration.Option.CsharpCodeSample);
            }
        }

        return builder.Build();
    }

    public string FormatQualityRule(CodeStyleRoslynQualityRuleConfiguration rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var builder = new MarkdownStringBuilder();

        builder.AddH2($"{rule.Rule.Title} ({rule.Rule.RuleId})");
        builder.AddEmptyLine();
        builder.AddText($"Severity: {rule.Severity}");
        builder.AddEmptyLine();
        builder.AddText(rule.Rule.Description);

        return builder.Build();
    }
}