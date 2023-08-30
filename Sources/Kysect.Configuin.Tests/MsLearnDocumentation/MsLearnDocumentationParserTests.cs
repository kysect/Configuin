using FluentAssertions;
using Kysect.Configuin.Core.MarkdownParsing;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnDocumentationParserTests
{
    private MsLearnDocumentationParser _parser = new(new PlainTextExtractor(MarkdownPipelineProvider.GetDefault()));

    [Test]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = File.ReadAllText(Path.Combine("Resources", "Ide0040.md"));

        var codeSample = """
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
                         """.Replace("\r\n", "\n", StringComparison.InvariantCultureIgnoreCase);

        RoslynStyleRuleOptionValue[] expectedOptionValues =
        {
            new RoslynStyleRuleOptionValue("always", "Prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("for_non_interface_members", "Prefer accessibility modifiers except for public interface members."),
            new RoslynStyleRuleOptionValue("never", "Do not prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("omit_if_default", "Prefer accessibility modifiers except if they are the default modifier.")
        };

        var expectedOption = new RoslynStyleRuleOption(
            Name: "dotnet_style_require_accessibility_modifiers",
            expectedOptionValues,
            DefaultValue: "for_non_interface_members",
            codeSample);

        var expected = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0040"),
            "Add accessibility modifiers",
            "Style",
            "This style rule concerns requiring accessibility modifiers in declarations.",
            string.Empty,
            new[] { expectedOption });

        RoslynStyleRule roslynStyleRule = _parser.ParseStyleRule(fileText);

        roslynStyleRule.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0003_0009_ReturnExpectedResult()
    {
        string fileText = File.ReadAllText(Path.Combine("Resources", "ide0003-ide0009.md"));

        var options = new RoslynStyleRuleOption[]
        {
            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_field",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer fields to be prefaced with `this.` in C# or `Me.` in Visual Basic"),
                    new RoslynStyleRuleOptionValue("false", "Prefer fields _not_ to be prefaced with `this.` or `Me.`"),
                },
                "false",
                string.Empty)
        };

        var ide0003 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0003"),
            "Remove this or Me qualification",
            "Style",
            """
            These two rules define whether or not you prefer the use of [this (C#)](../../../csharp/language-reference/keywords/this.md) and `Me.` (Visual Basic) qualifiers. To enforce that the qualifiers *aren't* present, set the severity of `IDE0003` to warning or error. To enforce that the qualifiers *are* present, set the severity of `IDE0009` to warning or error.
            
            For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable `IDE0009` and set the options `dotnet_style_qualification_for_field` and `dotnet_style_qualification_for_property` to `true`. However, this configuration would not flag methods and events that *do* have `this` and `Me` qualifiers. To also enforce that methods and events *don't* have qualifiers, enable `IDE0003`.
            """,
            string.Empty,
            options);

        var ide0009 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0009"),
            "**Add** `this` or `Me` qualification",
            "Style",
            """
            These two rules define whether or not you prefer the use of [this (C#)](../../../csharp/language-reference/keywords/this.md) and `Me.` (Visual Basic) qualifiers. To enforce that the qualifiers *aren't* present, set the severity of `IDE0003` to warning or error. To enforce that the qualifiers *are* present, set the severity of `IDE0009` to warning or error.

            For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable `IDE0009` and set the options `dotnet_style_qualification_for_field` and `dotnet_style_qualification_for_property` to `true`. However, this configuration would not flag methods and events that *do* have `this` and `Me` qualifiers. To also enforce that methods and events *don't* have qualifiers, enable `IDE0003`.
            """,
            string.Empty,
            options);

        IReadOnlyCollection<RoslynStyleRule> roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().ContainInOrder(ide0003, ide0009);
    }

    [Test]
    public void ParseQualityRule_CS1064_ReturnExpectedResult()
    {
        string fileText = File.ReadAllText(Path.Combine("Resources", "Ca1064.md"));
        // TODO: parse description
        var expected = new RoslynQualityRule(
            RoslynRuleId.Parse("CA1064"),
            string.Empty,
            "Design",
            string.Empty);

        RoslynQualityRule qualityRule = _parser.ParseQualityRule(fileText);

        qualityRule.Should().BeEquivalentTo(expected);
    }

    // TODO: remove ignore
    [Test]
    [Ignore("Need to fix all related problems")]
    public void Parse_MsDocsRepository_FinishWithoutError()
    {
        var repositoryPathProvider = new MsLearnDocumentationInfoLocalProvider(Constants.GetPathToMsDocsRoot());

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = repositoryPathProvider.Provide();
        RoslynRules roslynRules = _parser.Parse(msLearnDocumentationRawInfo);

        // TODO: add asserts
        Assert.Pass();
    }
}