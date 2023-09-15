using FluentAssertions;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigTemplateGeneratorTests
{
    private readonly EditorConfigTemplateGenerator _editorConfigTemplateGenerator;

    public EditorConfigTemplateGeneratorTests()
    {
        _editorConfigTemplateGenerator = new EditorConfigTemplateGenerator(TestLogger.ProviderForTests());
    }

    [Test]
    public void GenerateTemplate_ForIDE0040_ReturnExpectedString()
    {
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        string expected = """
                          ## Add accessibility modifiers (IDE0040)
                          ## This style rule concerns requiring accessibility modifiers in declarations.
                          # dotnet_diagnostic.IDE0040.severity = 
                          
                          ## Options:
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

        string generateTemplate = _editorConfigTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }

    [Test]
    public void GenerateTemplate_ForCA1064_ReturnExpectedString()
    {
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .Build();

        string expected = """
                          ## Exceptions should be public (CA1064)
                          ## An internal exception is only visible inside its own internal scope. After the exception falls outside the internal scope, only the base exception can be used to catch the exception. If the internal exception is inherited from xref:System.Exception, xref:System.SystemException, or xref:System.ApplicationException, the external code will not have sufficient information to know what to do with the exception.
                          ## But, if the code has a public exception that later is used as the base for an internal exception, it is reasonable to assume the code further out will be able to do something intelligent with the base exception. The public exception will have more information than what is provided by xref:System.Exception, xref:System.SystemException, or xref:System.ApplicationException.
                          # dotnet_diagnostic.CA1064.severity = 

                          
                          """;

        string generateTemplate = _editorConfigTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }
}