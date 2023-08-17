using Kysect.Configuin.Core.EditorConfigParsing.Rules;
using Kysect.Configuin.Core.IniParsing;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.EditorConfigParsing;

public class EditorConfigRuleParser : IEditorConfigRuleParser
{
    private readonly IniParser _iniParser = new IniParser();

    private readonly HashSet<string> _generalRuleKeys;

    public EditorConfigRuleParser()
    {
        // TODO: Investigate other rules
        _generalRuleKeys = new HashSet<string>
        {
            "tab_width",
            "indent_size",
            "end_of_line"
        };
    }

    public EditorConfigRuleSet Parse(string content)
    {
        IReadOnlyCollection<IniFileLine> lines = _iniParser.Parse(content);

        var rules = lines
            .Select(ParseRule)
            .ToList();

        return new EditorConfigRuleSet(rules);
    }

    private IEditorConfigRule ParseRule(IniFileLine line)
    {
        if (_generalRuleKeys.Contains(line.Key))
            return new GeneralEditorConfigRule(line.Key, line.Value);

        bool isRoslynSeverityRule = line.Key.StartsWith("dotnet_diagnostic.");
        if (isRoslynSeverityRule)
            return ParseRoslynSeverityRule(line);

        // TODO: remove rule that force StringComparison for string comparing from project .editorconfig
        bool isCompositeKeyRule = line.Key.Contains('.', StringComparison.InvariantCultureIgnoreCase);
        if (isCompositeKeyRule)
            return ParseCompositeKeyRule(line);

        return ParseRoslynOptionRule(line);
    }

    private static IEditorConfigRule ParseRoslynSeverityRule(IniFileLine line)
    {
        string[] keyParts = line.Key.Split('.');

        if (keyParts.Length != 3)
            throw new ArgumentException($"Incorrect rule key: {line.Key}");

        if (!string.Equals(keyParts[2], "severity", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expect postfix .severity for diagnostic rule but was {keyParts[2]}");

        if (!Enum.TryParse(line.Value, true, out RoslynRuleSeverity severity))
            throw new ArgumentException($"Cannot parse severity from {line.Value}");

        var ruleId = RoslynRuleId.Parse(keyParts[1]);
        return new RoslynSeverityEditorConfigRule(ruleId, severity);
    }

    private static IEditorConfigRule ParseCompositeKeyRule(IniFileLine line)
    {
        string[] keyParts = line.Key.Split('.');
        return new CompositeRoslynOptionEditorConfigRule(keyParts, line.Value, Severity: null);
    }

    private static IEditorConfigRule ParseRoslynOptionRule(IniFileLine line)
    {
        bool containsSeverityInValue = line.Value.Contains(':', StringComparison.InvariantCultureIgnoreCase);
        if (containsSeverityInValue)
        {
            string[] valueParts = line.Value.Split(':', 2);
            if (!Enum.TryParse(valueParts[1], true, out RoslynRuleSeverity severity))
                throw new ArgumentException($"Cannot parse severity from {valueParts[1]}");

            return new RoslynOptionEditorConfigRule(line.Key, valueParts[0], severity);
        }

        return new RoslynOptionEditorConfigRule(line.Key, line.Value, Severity: null);
    }
}