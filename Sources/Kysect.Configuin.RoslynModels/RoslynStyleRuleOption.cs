namespace Kysect.Configuin.RoslynModels;

public record RoslynStyleRuleOption(
    string Name,
    IReadOnlyCollection<RoslynStyleRuleOptionValue> Values,
    string? DefaultValue,
    string? CsharpCodeSample);