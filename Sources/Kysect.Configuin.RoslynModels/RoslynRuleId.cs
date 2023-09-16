using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;

namespace Kysect.Configuin.RoslynModels;

public readonly record struct RoslynRuleId(RoslynRuleType Type, int Id) : IComparable<RoslynRuleId>
{
    public static RoslynRuleId Parse(string value)
    {
        value.ThrowIfNull();

        int ParseInt(string intAsString)
        {
            if (int.TryParse(intAsString, out int parsedInt))
                return parsedInt;

            throw new ConfiguinException($"Value {value} is not valid rule identifier.");
        }

        // CA1234
        string qualityRulePrefix = "CA";
        if (value.StartsWith(qualityRulePrefix, StringComparison.InvariantCultureIgnoreCase))
        {
            string id = value.WithoutPrefix(qualityRulePrefix);
            return new RoslynRuleId(RoslynRuleType.QualityRule, ParseInt(id));
        }

        // IDE1234
        string styleRulePrefix = "IDE";
        if (value.StartsWith(styleRulePrefix, StringComparison.InvariantCultureIgnoreCase))
        {
            string id = value.WithoutPrefix(styleRulePrefix);
            return new RoslynRuleId(RoslynRuleType.StyleRule, ParseInt(id));
        }

        throw new ArgumentException($"String {value} is not valid Roslyn rule id");
    }

    public override string ToString()
    {
        return $"{Type.ToAlias()}{Id:D4}";
    }

    public int CompareTo(RoslynRuleId other)
    {
        int typeComparison = Type.CompareTo(other.Type);
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