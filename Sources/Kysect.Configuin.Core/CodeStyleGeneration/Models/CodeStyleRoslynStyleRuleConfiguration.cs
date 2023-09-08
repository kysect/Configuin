using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public record CodeStyleRoslynStyleRuleConfiguration(
        RoslynStyleRule Rule,
        RoslynRuleSeverity Severity,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> Options)
    : ICodeStyleElement;