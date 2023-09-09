using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public record CodeStyleRoslynQualityRuleConfiguration(
    RoslynQualityRule Rule,
    RoslynRuleSeverity Severity)
    : ICodeStyleElement;