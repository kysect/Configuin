using Kysect.CommonLib.Exceptions;

namespace Kysect.Configuin.RoslynModels;

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
            _ => throw SwitchDefaultExceptions.OnUnexpectedType(ruleType)
        };
    }
}