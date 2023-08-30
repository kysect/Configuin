namespace Kysect.Configuin.Core.RoslynRuleModels;

public readonly struct RoslynRuleIdRange
{
    public RoslynRuleId Start { get; }
    public RoslynRuleId End { get; }

    public static RoslynRuleIdRange Parse(string value)
    {
        string[] parts = value.Split("-", 2);
        var start = RoslynRuleId.Parse(parts[0]);
        var end = RoslynRuleId.Parse(parts[1]);
        return new RoslynRuleIdRange(start, end);
    }

    public RoslynRuleIdRange(RoslynRuleId start, RoslynRuleId end)
    {
        // TODO: add validation

        Start = start;
        End = end;
    }

    public IEnumerable<RoslynRuleId> Enumerate()
    {
        for (int i = Start.Id; i <= End.Id; i++)
            yield return new RoslynRuleId(Start.Type, i);
    }
}