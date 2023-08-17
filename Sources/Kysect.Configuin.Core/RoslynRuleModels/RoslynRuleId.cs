using Kysect.Configuin.Common;

namespace Kysect.Configuin.Core.RoslynRuleModels;

public readonly struct RoslynRuleId
{
    public RoslynRuleType Type { get; }
    public int Id { get; }

    public static RoslynRuleId Parse(string value)
    {
        // CA1234
        string qualityRulePrefix = "CA";
        if (value.StartsWith(qualityRulePrefix, StringComparison.InvariantCultureIgnoreCase))
        {
            string id = value.WithoutPrefix(qualityRulePrefix);
            return new RoslynRuleId(RoslynRuleType.QualityRule, int.Parse(id));
        }

        // IDE1234
        string styleRulePrefix = "IDE";
        if (value.StartsWith(styleRulePrefix, StringComparison.InvariantCultureIgnoreCase))
        {
            string id = value.WithoutPrefix(styleRulePrefix);
            return new RoslynRuleId(RoslynRuleType.StyleRule, int.Parse(id));
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
        return $"{Type}{Id:D4}";
    }
}