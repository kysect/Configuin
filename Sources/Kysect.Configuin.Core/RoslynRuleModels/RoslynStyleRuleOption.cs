namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynStyleRuleOption
{
    public string Name { get; }
    public IReadOnlyCollection<RoslynStyleRuleOptionValue> Options { get; }

    public RoslynStyleRuleOption(string name, IReadOnlyCollection<RoslynStyleRuleOptionValue> options)
    {
        Name = name;
        Options = options;
    }
}