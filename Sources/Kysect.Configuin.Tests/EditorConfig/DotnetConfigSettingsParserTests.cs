using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.EditorConfig;

public class DotnetConfigSettingsParserTests
{
    private readonly DotnetConfigSettingsParser _parser = new DotnetConfigSettingsParser(TestLogger.ProviderForTests());
    private readonly EditorConfigDocumentParser _documentParser = new EditorConfigDocumentParser();

    [Fact]
    public void Parse_TabWidth_ReturnGeneralEditorRule()
    {
        string content = "tab_width = 4";
        var expected = new GeneralEditorConfigSetting(Key: "tab_width", Value: "4");

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(content));

        dotnetConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Fact]
    public void Parse_DotnetDiagnosticSeverity_ReturnRoslyntSeverityRule()
    {
        string content = "dotnet_diagnostic.IDE0001.severity = warning";
        var expected = new RoslynSeverityEditorConfigSetting(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning);

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(content));

        dotnetConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Fact]
    public void Parse_KeyWithDots_ReturnCompositeOptionRule()
    {
        string content = "dotnet_naming_style.camel_case_style.capitalization = camel_case";
        var expected = new CompositeRoslynOptionEditorConfigSetting(
            KeyParts: new[] { "dotnet_naming_style", "camel_case_style", "capitalization" },
            Value: "camel_case", Severity: null);

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(content));

        dotnetConfigSettings.Settings
            .Should().HaveCount(1)
            .And.ContainEquivalentOf(expected);
    }

    [Fact]
    public void Parse_StyleOption_ReturnOptionRule()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigSetting(
            Key: "csharp_style_var_when_type_is_apparent",
            Value: "true");

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(content));

        dotnetConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Fact]
    public void Parse_StyleOptionWithSeverity_ReturnOptionRuleWithSeverity()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigSetting(
            Key: "csharp_style_var_when_type_is_apparent",
            Value: "true");

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(content));

        dotnetConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Fact]
    public void Parse_EditorConfigFile_ReturnWithoutErrors()
    {
        string fileText = File.ReadAllText(Path.Combine("Resources", "Editor-config-sample.ini"));

        DotnetConfigSettings dotnetConfigSettings = _parser.Parse(_documentParser.Parse(fileText));

        // TODO: add more asserts
        dotnetConfigSettings.Settings
            .Should().HaveCount(392);
    }
}