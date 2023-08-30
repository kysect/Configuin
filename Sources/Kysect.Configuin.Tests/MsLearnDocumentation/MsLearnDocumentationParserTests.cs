﻿using FluentAssertions;
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
    private MsLearnDocumentationParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MsLearnDocumentationParser(new PlainTextExtractor(MarkdownPipelineProvider.GetDefault()));
    }

    [Test]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = File.ReadAllText(Path.Combine("MsLearnDocumentation", "Resources", "Ide0040.md"));
        RoslynRuleId expected = RoslynRuleId.Parse("IDE0040");

        RoslynStyleRule roslynStyleRule = _parser.ParseStyleRule(fileText);

        roslynStyleRule.RuleId
            .Should().Be(expected);

        roslynStyleRule.Title
            .Should().Be("Add accessibility modifiers");

        roslynStyleRule.Category
            .Should().Be("Style");

        roslynStyleRule.Options
            .Should().HaveCount(1);

        roslynStyleRule.Options.Single().Name
            .Should().Be("dotnet_style_require_accessibility_modifiers");

        roslynStyleRule.Options.Single().Options
            .Should().Contain(new RoslynStyleRuleOptionValue("always", "Prefer accessibility modifiers to be specified."))
            .And.Contain(new RoslynStyleRuleOptionValue("for_non_interface_members", "Prefer accessibility modifiers except for public interface members."))
            .And.Contain(new RoslynStyleRuleOptionValue("never", "Do not prefer accessibility modifiers to be specified."))
            .And.Contain(new RoslynStyleRuleOptionValue("omit_if_default", "Prefer accessibility modifiers except if they are the default modifier."));

        roslynStyleRule.Options.Single().DefaultValue
            .Should().Be("for_non_interface_members");

        // TODO: add method Be() with ignoreEndOfLine or smth like this
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

        roslynStyleRule.Options.Single().CsharpCodeSample
            .Should().Be(codeSample);
    }

    [Test]
    public void ParseQualityRule_CS1064_ReturnExpectedResult()
    {
        string fileText = File.ReadAllText(Path.Combine("MsLearnDocumentation", "Resources", "Ca1064.md"));
        RoslynRuleId expected = RoslynRuleId.Parse("CA1064");

        RoslynQualityRule qualityRule = _parser.ParseQualityRule(fileText);

        qualityRule.RuleId
            .Should().Be(expected);

        qualityRule.Category
            .Should().Be("Design");

        // TODO: parse description
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