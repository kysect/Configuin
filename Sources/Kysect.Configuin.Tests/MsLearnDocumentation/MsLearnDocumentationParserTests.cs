using FluentAssertions;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnDocumentationParserTests
{
    private static readonly MsLearnRepositoryPathProvider MsLearnRepositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();

    private readonly MsLearnDocumentationParser _parser = new MsLearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());

    [Test]
    public void ParseStyleRule_IDE0001_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0001.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0001();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0040.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0040();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0003_0009_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0003-ide0009.md");
        RoslynStyleRule ide0003 = WellKnownRoslynRuleDefinitions.Ide0003();
        RoslynStyleRule ide0009 = WellKnownRoslynRuleDefinitions.Ide0009();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Rules.Should().HaveCount(2);
        roslynStyleRules.Rules.ElementAt(0).Should().BeEquivalentTo(ide0003);
        roslynStyleRules.Rules.ElementAt(1).Should().BeEquivalentTo(ide0009);
    }

    [Test]
    public void ParseQualityRule_CA1064_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1064.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1064();

        IReadOnlyCollection<RoslynQualityRule> qualityRule = _parser.ParseQualityRules(fileText);

        qualityRule.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseQualityRule_CA1865_CA1867_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1865-ca1867.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1865();

        IReadOnlyCollection<RoslynQualityRule> qualityRules = _parser.ParseQualityRules(fileText);

        qualityRules.Should().HaveCount(3);
        qualityRules.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Parse_DotnetFormattingOptions_ReturnExpectedResult()
    {
        string pathToDotnetFormattingFile = MsLearnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string fileContent = File.ReadAllText(pathToDotnetFormattingFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(2);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_sort_system_directives_first());
        roslynStyleRuleOptions.ElementAt(1).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_separate_import_directive_groups());
    }

    [Test]
    public void Parse_CsharpFormattingOptions_ReturnExpectedResult()
    {
        string pathToFile = MsLearnRepositoryPathProvider.GetPathToSharpFormattingFile();
        string fileContent = File.ReadAllText(pathToFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(37);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.csharp_new_line_before_open_brace());
    }

    [Test]
    [Ignore("Issue #33")]
    public void Parse_CodeStyleRefactoringOptions_ReturnExpectedResult()
    {
        string pathToFile = string.Empty;
        string fileContent = File.ReadAllText(pathToFile);

        IReadOnlyCollection<RoslynStyleRuleOption> codeStyleRefactoringOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        codeStyleRefactoringOptions.Should().HaveCount(1);
        codeStyleRefactoringOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_style_operator_placement_when_wrapping);
    }

    [Test]
    public void Parse_MsDocsRepository_FinishWithoutError()
    {
        MsLearnDocumentationInfoLocalReader repositoryPathReader = TestImplementations.CreateDocumentationInfoLocalProvider();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = repositoryPathReader.Provide(Constants.GetPathToMsDocsRoot());
        RoslynRules roslynRules = _parser.Parse(msLearnDocumentationRawInfo);

        // TODO: add asserts
        Assert.Pass();
    }

    private static string GetIdeDescription(string fileName)
    {
        string path = Path.Combine(MsLearnRepositoryPathProvider.GetPathToStyleRules(), fileName);
        return File.ReadAllText(path);
    }

    private static string GetPathToCa(string fileName)
    {
        string path = Path.Combine(MsLearnRepositoryPathProvider.GetPathToQualityRules(), fileName);
        return File.ReadAllText(path);
    }
}