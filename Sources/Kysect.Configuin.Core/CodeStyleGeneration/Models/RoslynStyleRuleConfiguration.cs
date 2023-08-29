using Kysect.Configuin.Core.EditorConfigParsing.Rules;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public class RoslynStyleRuleConfiguration : ICodeStyleElement
{
    public RoslynStyleRule Rule { get; }
    public RoslynRuleSeverity Severity { get; }
    public IReadOnlyCollection<RoslynOptionConfiguration> Options { get; }

    public RoslynStyleRuleConfiguration(RoslynStyleRule rule, RoslynRuleSeverity severity, IReadOnlyCollection<RoslynOptionConfiguration> options)
    {
        Rule = rule;
        Severity = severity;
        Options = options;
    }
}