using Kysect.Configuin.Core.EditorConfigParsing.Settings;
using Kysect.Configuin.Core.IniParsing;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.EditorConfigParsing;

public class EditorConfigSettingsParser : IEditorConfigSettingsParser
{
    private readonly IniParser _iniParser = new IniParser();

    private readonly HashSet<string> _generalRuleKeys;

    public EditorConfigSettingsParser()
    {
        // TODO: Investigate other rules
        _generalRuleKeys = new HashSet<string>
        {
            "tab_width",
            "indent_size",
            "end_of_line"
        };
    }

    public EditorConfigSettings Parse(string content)
    {
        IReadOnlyCollection<IniFileLine> iniFileLines = _iniParser.Parse(content);

        var rules = iniFileLines
            .Select(ParseSetting)
            .ToList();

        return new EditorConfigSettings(rules);
    }

    private IEditorConfigSetting ParseSetting(IniFileLine line)
    {
        if (_generalRuleKeys.Contains(line.Key))
            return new GeneralEditorConfigSetting(line.Key, line.Value);

        bool isSeveritySetting = line.Key.StartsWith("dotnet_diagnostic.");
        if (isSeveritySetting)
            return ParseSeveritySetting(line);

        bool isCompositeKeyRule = line.Key.Contains('.');
        if (isCompositeKeyRule)
            return ParseCompositeKeySetting(line);

        return ParseOptionSetting(line);
    }

    private static RoslynSeverityEditorConfigSetting ParseSeveritySetting(IniFileLine line)
    {
        string[] keyParts = line.Key.Split('.');

        if (keyParts.Length != 3)
            throw new ArgumentException($"Incorrect rule key: {line.Key}");

        if (!string.Equals(keyParts[2], "severity", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expect postfix .severity for diagnostic rule but was {keyParts[2]}");

        if (!Enum.TryParse(line.Value, true, out RoslynRuleSeverity severity))
            throw new ArgumentException($"Cannot parse severity from {line.Value}");

        var ruleId = RoslynRuleId.Parse(keyParts[1]);
        return new RoslynSeverityEditorConfigSetting(ruleId, severity);
    }

    private static CompositeRoslynOptionEditorConfigSetting ParseCompositeKeySetting(IniFileLine line)
    {
        string[] keyParts = line.Key.Split('.');
        return new CompositeRoslynOptionEditorConfigSetting(keyParts, line.Value, Severity: null);
    }

    private static RoslynOptionEditorConfigSetting ParseOptionSetting(IniFileLine line)
    {
        return new RoslynOptionEditorConfigSetting(line.Key, line.Value);
    }
}