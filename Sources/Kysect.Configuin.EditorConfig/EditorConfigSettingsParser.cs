using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.EditorConfig;

public class EditorConfigSettingsParser : IEditorConfigSettingsParser
{
    private readonly ILogger _logger;

    private readonly HashSet<string> _generalRuleKeys;
    private readonly EditorConfigDocumentParser _editorConfigDocumentParser;

    public EditorConfigSettingsParser(ILogger logger)
    {
        _logger = logger;

        // TODO: Investigate other rules
        _generalRuleKeys = new HashSet<string>
        {
            "tab_width",
            "indent_size",
            "end_of_line"
        };
        _editorConfigDocumentParser = new EditorConfigDocumentParser();
    }

    public EditorConfigSettings Parse(string content)
    {
        _logger.LogInformation("Parse .editorconfig file");

        ArgumentNullException.ThrowIfNull(content);

        EditorConfigDocument editorConfigDocument = _editorConfigDocumentParser.Parse(content);
        List<IEditorConfigSetting> settings = editorConfigDocument
            .DescendantNodes()
            .OfType<EditorConfigPropertyNode>()
            .Select(ParseSetting)
            .ToList();

        return new EditorConfigSettings(settings);
    }

    private IEditorConfigSetting ParseSetting(EditorConfigPropertyNode line)
    {
        if (_generalRuleKeys.Contains(line.Key.Value))
            return new GeneralEditorConfigSetting(line.Key.Value, line.Value.Value);

        bool isSeveritySetting = line.Key.Value.StartsWith("dotnet_diagnostic.");
        if (isSeveritySetting)
            return ParseSeveritySetting(line);

        bool isCompositeKeyRule = line.Key.Value.Contains('.');
        if (isCompositeKeyRule)
            return ParseCompositeKeySetting(line);

        return ParseOptionSetting(line);
    }

    private static RoslynSeverityEditorConfigSetting ParseSeveritySetting(EditorConfigPropertyNode line)
    {
        string[] keyParts = line.Key.Value.Split('.');

        if (keyParts.Length != 3)
            throw new ArgumentException($"Incorrect rule key: {line.Key.Value}");

        if (!string.Equals(keyParts[2], "severity", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expect postfix .severity for diagnostic rule but was {keyParts[2]}");

        if (!Enum.TryParse(line.Value.Value, true, out RoslynRuleSeverity severity))
            throw new ArgumentException($"Cannot parse severity from {line.Value.Value}");

        var ruleId = RoslynRuleId.Parse(keyParts[1]);
        return new RoslynSeverityEditorConfigSetting(ruleId, severity);
    }

    private static CompositeRoslynOptionEditorConfigSetting ParseCompositeKeySetting(EditorConfigPropertyNode line)
    {
        string[] keyParts = line.Key.Value.Split('.');
        return new CompositeRoslynOptionEditorConfigSetting(keyParts, line.Value.Value, Severity: null);
    }

    private static RoslynOptionEditorConfigSetting ParseOptionSetting(EditorConfigPropertyNode line)
    {
        return new RoslynOptionEditorConfigSetting(line.Key.Value, line.Value.Value);
    }
}