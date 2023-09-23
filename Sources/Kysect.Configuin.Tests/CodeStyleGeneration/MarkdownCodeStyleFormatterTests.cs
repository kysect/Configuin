using FluentAssertions;
using Kysect.Configuin.CodeStyleDoc.Markdown;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class MarkdownCodeStyleFormatterTests
{
    private readonly MarkdownCodeStyleFormatter _formatter = new MarkdownCodeStyleFormatter(TestLogger.ProviderForTests());

    [Test]
    public void FormatStyleRule_ForIDE0001_ReturnExpected()
    {
        const string expected = """
                                ## Simplify name (IDE0001)
                                
                                Severity: Warning
                                
                                This rule concerns the use of simplified type names in declarations and executable code, when possible. You can remove unnecessary name qualification to simplify code and improve readability.
                                
                                ```csharp
                                using System.IO;
                                class C
                                {
                                    // IDE0001: 'System.IO.FileInfo' can be simplified to 'FileInfo'
                                    System.IO.FileInfo file;
                                
                                    // Fixed code
                                    FileInfo file;
                                }
                                ```
                                
                                """;

        RoslynStyleRuleGroup ide0040 = WellKnownRoslynRuleDefinitions.IDE0001();

        var styleRoslynStyleRuleConfiguration = new CodeStyleRoslynStyleRuleConfiguration(
            ide0040.Rules.Single(),
            RoslynRuleSeverity.Warning,
            Options: Array.Empty<CodeStyleRoslynOptionConfiguration>(),
            ide0040.Overview,
            ide0040.Example);

        string formatterRule = _formatter.FormatStyleRule(styleRoslynStyleRuleConfiguration);

        formatterRule.Should().Be(expected);
    }

    [Test]
    public void FormatStyleRule_ForIDE0040_ReturnExpected()
    {
        const string expected = """
                                 ## Add accessibility modifiers (IDE0040)

                                 Severity: Warning

                                 This style rule concerns requiring accessibility modifiers in declarations.

                                 ### dotnet_style_require_accessibility_modifiers = always

                                 ```csharp
                                 // dotnet_style_require_accessibility_modifiers = always
                                 // dotnet_style_require_accessibility_modifiers = for_non_interface_members
                                 class MyClass
                                 {
                                     private const string thisFieldIsConst = "constant";
                                 }

                                 // dotnet_style_require_accessibility_modifiers = never
                                 class MyClass
                                 {
                                     const string thisFieldIsConst = "constant";
                                 }
                                 ```
                                 
                                 """;

        RoslynStyleRuleGroup ide0040 = WellKnownRoslynRuleDefinitions.IDE0040();
        var optionConfiguration = new CodeStyleRoslynOptionConfiguration(ide0040.Options.Single(), "always");

        var styleRoslynStyleRuleConfiguration = new CodeStyleRoslynStyleRuleConfiguration(
            ide0040.Rules.Single(),
            RoslynRuleSeverity.Warning,
            new[] { optionConfiguration },
            ide0040.Overview,
            ide0040.Example);

        string formatterRule = _formatter.FormatStyleRule(styleRoslynStyleRuleConfiguration);

        formatterRule.Should().Be(expected);
    }

    [Test]
    public void FormatQualityRule_ForCA1064_ReturnExpected()
    {
        // TODO: add description
        const string expected = """
                                ## Exceptions should be public (CA1064)

                                Severity: Warning

                                An internal exception is only visible inside its own internal scope. After the exception falls outside the internal scope, only the base exception can be used to catch the exception. If the internal exception is inherited from System.Exception, System.SystemException, or System.ApplicationException, the external code will not have sufficient information to know what to do with the exception.
                                But, if the code has a public exception that later is used as the base for an internal exception, it is reasonable to assume the code further out will be able to do something intelligent with the base exception. The public exception will have more information than what is provided by System.Exception, System.SystemException, or System.ApplicationException.
                                
                                """;

        RoslynQualityRule ca1064 = WellKnownRoslynRuleDefinitions.CA1064();
        var qualityRuleConfiguration = new CodeStyleRoslynQualityRuleConfiguration(
            ca1064,
            RoslynRuleSeverity.Warning);

        string formatterRule = _formatter.FormatQualityRule(qualityRuleConfiguration);

        formatterRule.Should().Be(expected);
    }
}