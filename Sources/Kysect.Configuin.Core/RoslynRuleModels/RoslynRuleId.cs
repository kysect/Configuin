using Kysect.Configuin.Common;

namespace Kysect.Configuin.Core.RoslynRuleModels;

public readonly struct RoslynRuleId
{
    public RoslynRuleType Type { get; }
    public int Id { get; }

    public static RoslynRuleId Parse(string value)
    {
        int ParseInt(string intAsString)
        {
            if (int.TryParse(intAsString, out int parsedInt))
                return parsedInt;

            throw new ConfiguinException($"Value {value} is not valid rule identifier.");
        }

        // CA1234
        string qualityRulePrefix = "CA";
        if (value.StartsWith(qualityRulePrefix))
        {
            string id = value.WithoutPrefix(qualityRulePrefix);
            return new RoslynRuleId(RoslynRuleType.QualityRule, ParseInt(id));
        }

        // IDE1234
        string styleRulePrefix = "IDE";
        if (value.StartsWith(styleRulePrefix))
        {
            string id = value.WithoutPrefix(styleRulePrefix);
            return new RoslynRuleId(RoslynRuleType.StyleRule, ParseInt(id));
        }

        throw new ArgumentException($"String {value} is not valid Roslyn rule id");
    }

    public RoslynRuleId(RoslynRuleType type, int id)
    {
        Type = type;
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is RoslynRuleId o)
            return Equals(o);

        return false;
    }

    public bool Equals(RoslynRuleId other)
    {
        return Type == other.Type && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Id);
    }

    public override string ToString()
    {
        return $"{Type.ToAlias()}{Id:D4}";
    }
}