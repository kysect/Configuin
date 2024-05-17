using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.Configuin.RoslynModels;

public static class RoslynNameRuleInfo
{
    public static RoslynRuleId RuleId { get; } = new RoslynRuleId("IDE", 1006);

    public static bool IsNameRuleOption(string optionName)
    {
        optionName.ThrowIfNull();

        return optionName.StartsWith("dotnet_naming_rule")
               || optionName.StartsWith("dotnet_naming_symbols")
               || optionName.StartsWith("dotnet_naming_style");

    }
}