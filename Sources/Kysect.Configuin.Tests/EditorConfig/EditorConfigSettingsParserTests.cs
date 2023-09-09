using FluentAssertions;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigSettingsParserTests
{
    private readonly EditorConfigSettingsParser _parser = new EditorConfigSettingsParser(TestLogger.ProviderForTests());

    [Test]
    public void Parse_TabWidth_ReturnGeneralEditorRule()
    {
        string content = "tab_width = 4";
        var expected = new GeneralEditorConfigSetting(Key: "tab_width", Value: "4");

        EditorConfigSettings editorConfigSettings = _parser.Parse(content);

        editorConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_DotnetDiagnosticSeverity_ReturnRoslyntSeverityRule()
    {
        string content = "dotnet_diagnostic.IDE0001.severity = warning";
        var expected = new RoslynSeverityEditorConfigSetting(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning);

        EditorConfigSettings editorConfigSettings = _parser.Parse(content);

        editorConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_KeyWithDots_ReturnCompositeOptionRule()
    {
        string content = "dotnet_naming_style.camel_case_style.capitalization = camel_case";
        var expected = new CompositeRoslynOptionEditorConfigSetting(
            KeyParts: new[] { "dotnet_naming_style", "camel_case_style", "capitalization" },
            Value: "camel_case", Severity: null);

        EditorConfigSettings editorConfigSettings = _parser.Parse(content);

        editorConfigSettings.Settings
            .Should().HaveCount(1)
            .And.ContainEquivalentOf(expected);
    }

    [Test]
    public void Parse_StyleOption_ReturnOptionRule()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigSetting(
            Key: "csharp_style_var_when_type_is_apparent",
            Value: "true");

        EditorConfigSettings editorConfigSettings = _parser.Parse(content);

        editorConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_StyleOptionWithSeverity_ReturnOptionRuleWithSeverity()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigSetting(
            Key: "csharp_style_var_when_type_is_apparent",
            Value: "true");

        EditorConfigSettings editorConfigSettings = _parser.Parse(content);

        editorConfigSettings.Settings
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_EditorConfigFile_ReturnWithoutErrors()
    {
        string fileText = File.ReadAllText(Path.Combine("Resources", "Editor-config-sample.ini"));

        EditorConfigSettings editorConfigSettings = _parser.Parse(fileText);

        // TODO: add more asserts
        editorConfigSettings.Settings
            .Should().HaveCount(393);
    }
}