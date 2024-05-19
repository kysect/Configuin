using Kysect.Configuin.DotnetConfig.Template;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.DotnetConfig;

public class DotnetConfigDocumentTemplateGeneratorTests
{
    private readonly DotnetConfigDocumentTemplateGenerator _dotnetConfigDocumentTemplateGenerator;

    public DotnetConfigDocumentTemplateGeneratorTests()
    {
        _dotnetConfigDocumentTemplateGenerator = new DotnetConfigDocumentTemplateGenerator(TestLogger.ProviderForTests());
    }

    [Fact]
    public void GenerateTemplate_ForIDE0001_ReturnExpectedString()
    {
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0001())
            .Build();

        string expected = """
                          ## Simplify name (IDE0001)
                          ## This rule concerns the use of simplified type names in declarations and executable code, when possible. You can remove unnecessary name qualification to simplify code and improve readability.
                          ## using System.IO;
                          ## class C
                          ## {
                          ##     // IDE0001: 'System.IO.FileInfo' can be simplified to 'FileInfo'
                          ##     System.IO.FileInfo file;
                          ## 
                          ##     // Fixed code
                          ##     FileInfo file;
                          ## }
                          # dotnet_diagnostic.IDE0001.severity = 


                          """;

        string generateTemplate = _dotnetConfigDocumentTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }

    [Fact]
    public void GenerateTemplate_ForIDE0007_0008_ReturnExpectedString()
    {
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0007_0008())
            .Build();

        string expected = """
                          ## Use var instead of explicit type (IDE0007)
                          ## Use explicit type instead of var (IDE0008)
                          ## These two style rules define whether the var keyword or an explicit type should be used in a variable declaration. To enforce that var is used, set the severity of IDE0007 to warning or error. To enforce that the explicit type is used, set the severity of IDE0008 to warning or error.
                          # dotnet_diagnostic.IDE0007.severity = 
                          # dotnet_diagnostic.IDE0008.severity = 

                          
                          """;

        string generateTemplate = _dotnetConfigDocumentTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }

    [Fact]
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

        string generateTemplate = _dotnetConfigDocumentTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }

    [Fact]
    public void GenerateTemplate_ForCA1064_ReturnExpectedString()
    {
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .Build();

        string expected = """
                          ## Exceptions should be public (CA1064)
                          ## An internal exception is only visible inside its own internal scope. After the exception falls outside the internal scope, only the base exception can be used to catch the exception. If the internal exception is inherited from System.Exception, System.SystemException, or System.ApplicationException, the external code will not have sufficient information to know what to do with the exception.
                          ## But, if the code has a public exception that later is used as the base for an internal exception, it is reasonable to assume the code further out will be able to do something intelligent with the base exception. The public exception will have more information than what is provided by System.Exception, System.SystemException, or System.ApplicationException.
                          # dotnet_diagnostic.CA1064.severity = 

                          
                          """;

        string generateTemplate = _dotnetConfigDocumentTemplateGenerator.GenerateTemplate(roslynRules);

        generateTemplate.Should().Be(expected);
    }
}