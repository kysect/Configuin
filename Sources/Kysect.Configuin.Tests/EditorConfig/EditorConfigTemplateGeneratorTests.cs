using FluentAssertions;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigTemplateGeneratorTests
{
    [Test]
    public void GenerateTemplate_ForIDE0040_ReturnExpectedString()
    {
        var editorConfigTemplateGenerator = new EditorConfigTemplateGenerator();
        var roslynRules = new RoslynRules(Array.Empty<RoslynQualityRule>(), new[] { WellKnownRoslynRuleDefinitions.IDE0040() });
        string expected = """
                          ## Add accessibility modifiers (IDE0040)
                          ## This style rule concerns requiring accessibility modifiers in declarations.
                          # dotnet_diagnostic.IDE0040.severity = 
                          ## IDE0040 options:
                          ## dotnet_style_require_accessibility_modifiers
                          ## - always - Prefer accessibility modifiers to be specified.
                          ## - for_non_interface_members - Prefer accessibility modifiers except for public interface members.
                          ## - never - Do not prefer accessibility modifiers to be specified.
                          ## - omit_if_default - Prefer accessibility modifiers except if they are the default modifier.
                          ## // dotnet_style_require_accessibility_modifiers = always
                          ## // dotnet_style_require_accessibility_modifiers = for_non_interface_members
                          ## class MyClass
                          ## {
                          ##     private const string thisFieldIsConst = "constant";
                          ## }
                          ## 
                          ## // dotnet_style_require_accessibility_modifiers = never
                          ## class MyClass
                          ## {
                          ##     const string thisFieldIsConst = "constant";
                          ## }
                          # dotnet_style_require_accessibility_modifiers = 
                          
                          """;

        string generateTemplate = editorConfigTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }
}