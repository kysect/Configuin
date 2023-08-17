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
        if (value.StartWithIgnoreCase(qualityRulePrefix))
        {
            string id = value.RemovePrefix(qualityRulePrefix);
            return new RoslynRuleId(RoslynRuleType.QualityRule, int.Parse(id));
        }

        // IDE1234
        string styleRulePrefix = "IDE";
        if (value.StartWithIgnoreCase(styleRulePrefix))
        {
            string id = value.RemovePrefix(styleRulePrefix);
            return new RoslynRuleId(RoslynRuleType.QualityRule, int.Parse(id));
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
}