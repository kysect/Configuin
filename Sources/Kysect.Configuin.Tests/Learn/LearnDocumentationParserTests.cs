using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnDocumentationParserTests
{
    private static readonly LearnRepositoryPathProvider LearnRepositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();

    private readonly LearnDocumentationParser _parser = new LearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());
    private readonly IRoslynRuleDocumentationParser _roslynRuleDocumentationParser;

    public LearnDocumentationParserTests()
    {
        _roslynRuleDocumentationParser = new LearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());
    }

    [Fact]
    public void ParseStyleRule_IDE0001_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0001.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0001();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0040.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0040();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Fact]
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

    [Fact]
    public void ParseQualityRule_CA1064_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1064.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1064();

        IReadOnlyCollection<RoslynQualityRule> qualityRule = _parser.ParseQualityRules(fileText);

        qualityRule.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseQualityRule_CA1865_CA1867_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1865-ca1867.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1865();

        IReadOnlyCollection<RoslynQualityRule> qualityRules = _parser.ParseQualityRules(fileText);

        qualityRules.Should().HaveCount(3);
        qualityRules.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_DotnetFormattingOptions_ReturnExpectedResult()
    {
        string pathToDotnetFormattingFile = LearnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string fileContent = File.ReadAllText(pathToDotnetFormattingFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(2);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_sort_system_directives_first());
        roslynStyleRuleOptions.ElementAt(1).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_separate_import_directive_groups());
    }

    [Fact]
    public void Parse_CsharpFormattingOptions_ReturnExpectedResult()
    {
        string pathToFile = LearnRepositoryPathProvider.GetPathToSharpFormattingFile();
        string fileContent = File.ReadAllText(pathToFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(37);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.csharp_new_line_before_open_brace());
    }

    [Fact(Skip = "Issue #33")]
    public void Parse_CodeStyleRefactoringOptions_ReturnExpectedResult()
    {
        string pathToFile = string.Empty;
        string fileContent = File.ReadAllText(pathToFile);

        IReadOnlyCollection<RoslynStyleRuleOption> codeStyleRefactoringOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        codeStyleRefactoringOptions.Should().HaveCount(1);
        codeStyleRefactoringOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_style_operator_placement_when_wrapping);
    }

    [Fact]
    public void Parse_MsDocsRepository_FinishWithoutError()
    {
        RoslynRules roslynRules = _roslynRuleDocumentationParser.Parse(Constants.GetPathToMsDocsRoot());

        roslynRules.QualityRules.Single(r => r.RuleId.ToString() == "CA2007").Options.Should().HaveCount(2);
        // TODO: add asserts
    }

    private static string GetIdeDescription(string fileName)
    {
        string path = Path.Combine(LearnRepositoryPathProvider.GetPathToStyleRules(), fileName);
        return File.ReadAllText(path);
    }

    private static string GetPathToCa(string fileName)
    {
        string path = Path.Combine(LearnRepositoryPathProvider.GetPathToQualityRules(), fileName);
        return File.ReadAllText(path);
    }
}