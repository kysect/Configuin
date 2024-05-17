using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;

namespace Kysect.Configuin.RoslynModels;

public readonly record struct RoslynRuleId(string RuleType, int Id) : IComparable<RoslynRuleId>
{
    public static RoslynRuleId Parse(string value)
    {
        value.ThrowIfNull();

        if (value.Length < 5)
            throw new ArgumentException($"Invalid Roslyn rule ID {value}");

        string code = value.Substring(value.Length - 4, 4);
        if (!int.TryParse(code, out int parsedCode))
            throw new ConfiguinException($"Value {value} is not valid rule identifier.");

        string type = value.Substring(0, value.Length - 4).ToUpper();
        return new RoslynRuleId(type, parsedCode);
    }

    public override string ToString()
    {
        return $"{RuleType}{Id:D4}";
    }

    public int CompareTo(RoslynRuleId other)
    {
        int typeComparison = string.Compare(RuleType, other.RuleType, StringComparison.Ordinal);
        if (typeComparison != 0)
            return typeComparison;
        return Id.CompareTo(other.Id);
    }

    public static bool operator <(RoslynRuleId left, RoslynRuleId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(RoslynRuleId left, RoslynRuleId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(RoslynRuleId left, RoslynRuleId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(RoslynRuleId left, RoslynRuleId right)
    {
        return left.CompareTo(right) >= 0;
    }
}