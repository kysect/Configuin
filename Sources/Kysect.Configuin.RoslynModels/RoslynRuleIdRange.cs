namespace Kysect.Configuin.RoslynModels;

public readonly struct RoslynRuleIdRange
{
    public RoslynRuleId Start { get; }
    public RoslynRuleId End { get; }

    public static RoslynRuleIdRange Parse(string value)
    {
        RoslynRuleId start;
        RoslynRuleId end;

        if (value.Contains("-"))
        {
            string[] parts = value.Split("-", 2);
            start = RoslynRuleId.Parse(parts[0]);
            end = RoslynRuleId.Parse(parts[1]);
        }
        else
        {
            start = RoslynRuleId.Parse(value);
            end = start;
        }

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