using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public record CodeStyleRoslynQualityRuleConfiguration(
    RoslynQualityRule Rule,
    RoslynRuleSeverity Severity)
    : ICodeStyleElement;