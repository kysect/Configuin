using Kysect.Configuin.Common;

namespace Kysect.Configuin.Core.RoslynRuleModels;

public struct RoslynRuleId
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
}