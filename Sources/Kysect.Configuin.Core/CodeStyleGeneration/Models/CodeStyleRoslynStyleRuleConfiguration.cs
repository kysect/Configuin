using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public record CodeStyleRoslynStyleRuleConfiguration(
        RoslynStyleRule Rule,
        RoslynRuleSeverity Severity,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> Options)
    : ICodeStyleElement;