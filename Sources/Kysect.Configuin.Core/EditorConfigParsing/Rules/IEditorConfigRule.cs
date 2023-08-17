namespace Kysect.Configuin.Core.EditorConfigParsing.Rules;

public interface IEditorConfigRule
{
}

public record CompositeRoslynOptionEditorConfigRule(
    IReadOnlyCollection<string> KeyParts,
    string Value,
    RoslynRuleSeverity? Severity) : IEditorConfigRule;

public record GeneralEditorConfigRule(
    string Key,
    string Value) : IEditorConfigRule;

public record RoslynOptionEditorConfigRule(
    string Key,
    string Value,
    RoslynRuleSeverity? Severity) : IEditorConfigRule;

public record RoslynSeverityEditorConfigRule(
    string RuleId,
    RoslynRuleSeverity Severity) : IEditorConfigRule;