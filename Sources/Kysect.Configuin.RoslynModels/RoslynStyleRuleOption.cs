namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRuleOption(
    string Name,
    IReadOnlyCollection<RoslynStyleRuleOptionValue> Options,
    string? DefaultValue,
    string? CsharpCodeSample);