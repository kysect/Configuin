using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.CodeStyleDoc.Models;

public record CodeStyleRoslynQualityRuleConfiguration(
    RoslynQualityRule Rule,
    RoslynRuleSeverity Severity)
    : ICodeStyleElement;