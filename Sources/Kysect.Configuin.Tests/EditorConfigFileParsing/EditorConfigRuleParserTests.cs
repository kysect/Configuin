using FluentAssertions;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.EditorConfigParsing.Rules;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfigFileParsing;

public class EditorConfigRuleParserTests
{
    private readonly EditorConfigRuleParser _parser = new EditorConfigRuleParser();

    [Test]
    public void Parse_TabWidth_ReturnGeneralEditorRule()
    {
        string content = "tab_width = 4";
        var expected = new GeneralEditorConfigRule("tab_width", "4");

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(content);

        editorConfigRuleSet.Rules
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_DotnetDiagnosticSeverity_ReturnRoslyntSeverityRule()
    {
        string content = "dotnet_diagnostic.IDE0001.severity = warning";
        var expected = new RoslynSeverityEditorConfigRule("IDE0001", RoslynRuleSeverity.Warning);

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(content);

        editorConfigRuleSet.Rules
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_KeyWithDots_ReturnCompositeOptionRule()
    {
        string content = "dotnet_naming_style.camel_case_style.capitalization = camel_case";
        var expected = new CompositeRoslynOptionEditorConfigRule(new[] { "dotnet_naming_style", "camel_case_style", "capitalization" }, "camel_case", Severity: null);

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(content);

        editorConfigRuleSet.Rules
            .Should().HaveCount(1)
            .And.ContainEquivalentOf(expected);
    }

    [Test]
    public void Parse_StyleOption_ReturnOptionRule()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigRule("csharp_style_var_when_type_is_apparent", "true", Severity: null);

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(content);

        editorConfigRuleSet.Rules
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_StyleOptionWithSeverity_ReturnOptionRuleWithSeverity()
    {
        string content = "csharp_style_var_when_type_is_apparent = true";
        var expected = new RoslynOptionEditorConfigRule("csharp_style_var_when_type_is_apparent", "true", Severity: null);

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(content);

        editorConfigRuleSet.Rules
            .Should().HaveCount(1)
            .And.Contain(expected);
    }

    [Test]
    public void Parse_EditorConfigFile_ReturnWithoutErrors()
    {
        string fileText = File.ReadAllText(Path.Combine("EditorConfigFileParsing", "Resources", "Editor-config-sample.ini"));

        EditorConfigRuleSet editorConfigRuleSet = _parser.Parse(fileText);

        editorConfigRuleSet.Rules
            .Should().HaveCount(400);
    }
}