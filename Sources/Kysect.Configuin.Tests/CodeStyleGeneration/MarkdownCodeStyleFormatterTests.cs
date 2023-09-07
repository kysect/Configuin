using FluentAssertions;
using Kysect.Configuin.Core.CodeStyleGeneration.Markdown;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class MarkdownCodeStyleFormatterTests
{
    private readonly MarkdownCodeStyleFormatter _formatter = new MarkdownCodeStyleFormatter(TestLogger.ProviderForTests());

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

        RoslynStyleRule ide0040 = WellKnownRoslynRuleDefinitions.IDE0040();
        var optionConfiguration = new CodeStyleRoslynOptionConfiguration(ide0040.Options.Single(), "always");

        var styleRoslynStyleRuleConfiguration = new CodeStyleRoslynStyleRuleConfiguration(
            ide0040,
            RoslynRuleSeverity.Warning,
            new[] { optionConfiguration });

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

                                """;

        RoslynQualityRule ca1064 = WellKnownRoslynRuleDefinitions.CA1064();
        var qualityRuleConfiguration = new CodeStyleRoslynQualityRuleConfiguration(
            ca1064,
            RoslynRuleSeverity.Warning);

        string formatterRule = _formatter.FormatQualityRule(qualityRuleConfiguration);

        formatterRule.Should().Be(expected);
    }
}