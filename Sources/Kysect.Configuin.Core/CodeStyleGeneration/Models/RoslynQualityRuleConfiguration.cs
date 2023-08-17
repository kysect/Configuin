using Kysect.Configuin.Core.EditorConfigParsing.Rules;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public class RoslynQualityRuleConfiguration : ICodeStyleElement
{
    public RoslynQualityRule Rule { get; }
    public RoslynRuleSeverity Severity { get; }

    public RoslynQualityRuleConfiguration(RoslynQualityRule rule, RoslynRuleSeverity severity)
    {
        Rule = rule;
        Severity = severity;
    }
}