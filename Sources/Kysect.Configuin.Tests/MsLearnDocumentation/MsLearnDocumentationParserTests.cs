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
    private MsLearnDocumentationParser _parser = new MsLearnDocumentationParser(new PlainTextExtractor(MarkdownPipelineProvider.GetDefault()));

    [Test]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0040.md");

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
                         """;

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

        var roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0003_0009_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription( "ide0003-ide0009.md");

        var options = new RoslynStyleRuleOption[]
        {
            // TODO: add dots to end of lines into source code and fix test
            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_field",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer fields to be prefaced with this. in C# or Me. in Visual Basic"),
                    new RoslynStyleRuleOptionValue("false", "Prefer fields not to be prefaced with this. or Me."),
                },
                "false",
                """
                // dotnet_style_qualification_for_field = true
                this.capacity = 0;
                
                // dotnet_style_qualification_for_field = false
                capacity = 0;
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_property",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer properties to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer properties not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_property = true
                this.ID = 0;
                
                // dotnet_style_qualification_for_property = false
                ID = 0;
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_method",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer methods to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer methods not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_method = true
                this.Display();
                
                // dotnet_style_qualification_for_method = false
                Display();
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_event",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer events to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer events not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_event = true
                this.Elapsed += Handler;
                
                // dotnet_style_qualification_for_event = false
                Elapsed += Handler;
                """),
        };

        string overview = """
                          These two rules define whether or not you prefer the use of this (C#) and Me. (Visual Basic) qualifiers. To enforce that the qualifiers aren't present, set the severity of IDE0003 to warning or error. To enforce that the qualifiers are present, set the severity of IDE0009 to warning or error.
                          For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable IDE0009 and set the options dotnet_style_qualification_for_field and dotnet_style_qualification_for_property to true. However, this configuration would not flag methods and events that do have this and Me qualifiers. To also enforce that methods and events don't have qualifiers, enable IDE0003.
                          """;

        var ide0003 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0003"),
            "Remove this or Me qualification",
            "Style",
            overview,
            string.Empty,
            options);

        var ide0009 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0009"),
            "Add this or Me qualification",
            "Style",
            overview,
            string.Empty,
            options);

        IReadOnlyCollection<RoslynStyleRule> roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().HaveCount(2);
        roslynStyleRules.ElementAt(0).Should().BeEquivalentTo(ide0003);
        roslynStyleRules.ElementAt(1).Should().BeEquivalentTo(ide0009);
    }

    [Test]
    public void ParseQualityRule_CA1064_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1064.md");

        // TODO: parse description
        var expected = new RoslynQualityRule(
            RoslynRuleId.Parse("CA1064"),
            string.Empty,
            "Design",
            string.Empty);

        IReadOnlyCollection<RoslynQualityRule> qualityRule = _parser.ParseQualityRules(fileText);

        qualityRule.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseQualityRule_CA1865_CA1867_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1865-ca1867.md");

        // TODO: parse description
        var expected = new RoslynQualityRule(
            RoslynRuleId.Parse("CA1865"),
            // TODO: parse name?
            ruleName: string.Empty,
            category: "Performance",
            // TODO: parse description?
            description: string.Empty);

        IReadOnlyCollection<RoslynQualityRule> qualityRules = _parser.ParseQualityRules(fileText);

        qualityRules.Should().HaveCount(3);
        qualityRules.ElementAt(0).Should().BeEquivalentTo(expected);
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

    private static string GetIdeDescription(string fileName)
    {
        string path = Path.Combine(new MsLearnRepositoryPathProvider(Constants.GetPathToMsDocsRoot()).GetPathToStyleRules(), fileName);
        return File.ReadAllText(path);
    }

    private static string GetPathToCa(string fileName)
    {
        string path = Path.Combine(new MsLearnRepositoryPathProvider(Constants.GetPathToMsDocsRoot()).GetPathToQualityRules(), fileName);
        return File.ReadAllText(path);
    }
}