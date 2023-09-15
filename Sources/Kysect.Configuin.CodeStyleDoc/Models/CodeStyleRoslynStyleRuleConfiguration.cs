using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.CodeStyleDoc.Models;

public record CodeStyleRoslynStyleRuleConfiguration(
        RoslynStyleRule Rule,
        RoslynRuleSeverity Severity,
        IReadOnlyCollection<CodeStyleRoslynOptionConfiguration> Options,
        string Overview,
        string? Example)
    : ICodeStyleElement;