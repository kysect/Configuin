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

        // TODO: add method Be() with ignoreEndOfLine or smth like this
        roslynStyleRule.Should().BeEquivalentTo(expected);
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