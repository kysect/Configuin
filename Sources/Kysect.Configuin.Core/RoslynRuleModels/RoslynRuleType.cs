using Kysect.CommonLib.Exceptions;

namespace Kysect.Configuin.Core.RoslynRuleModels;

public enum RoslynRuleType
{
    StyleRule,
    QualityRule
}

public static class RoslynRuleTypeExtensions
{
    public static string ToAlias(this RoslynRuleType ruleType)
    {
        return ruleType switch
        {
            RoslynRuleType.StyleRule => "IDE",
            RoslynRuleType.QualityRule => "CA",
            _ => throw SwitchDefaultException.OnUnexpectedEnum(nameof(ruleType), ruleType)
        };
    }
}