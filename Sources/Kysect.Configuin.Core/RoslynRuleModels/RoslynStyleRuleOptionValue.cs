namespace Kysect.Configuin.Core.RoslynRuleModels;

public class RoslynStyleRuleOptionValue
{
    public string Value { get; }
    public string Description { get; }

    public RoslynStyleRuleOptionValue(string value, string description)
    {
        Value = value;
        Description = description;
    }
}