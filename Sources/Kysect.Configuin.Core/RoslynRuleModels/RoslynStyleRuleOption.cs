namespace Kysect.Configuin.Core.RoslynRuleModels;

public record RoslynStyleRuleOption(
    string Name,
    IReadOnlyCollection<RoslynStyleRuleOptionValue> Options,
    string DefaultValue,
    string CsharpCodeSample);