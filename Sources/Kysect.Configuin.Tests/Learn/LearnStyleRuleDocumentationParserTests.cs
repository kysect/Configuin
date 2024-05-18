using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.ContentParsing;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnStyleRuleDocumentationParserTests
{
    private static readonly LearnRepositoryPathProvider LearnRepositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();

    private readonly LearnStyleRuleDocumentationParser _parser;
    private readonly LearnDocumentationPreprocessor _preprocessor;

    public LearnStyleRuleDocumentationParserTests()
    {
        IMarkdownTextExtractor markdownTextExtractor = TestImplementations.GetTextExtractor();
        var learnTableParser = new LearnTableParser();
        var markdownTableParser = new MarkdownTableParser(markdownTextExtractor);
        var markdownHeadedBlockParser = new LearnMarkdownBlockParser(markdownTextExtractor, markdownTableParser, learnTableParser);
        _parser = new LearnStyleRuleDocumentationParser(markdownHeadedBlockParser, markdownTableParser, markdownTextExtractor, learnTableParser);
        _preprocessor = new LearnDocumentationPreprocessor();
    }

    [Fact]
    public void ParseStyleRule_IDE0001_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0001.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0001();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText, []);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0040.md");

        RoslynStyleRuleGroup expected = WellKnownRoslynRuleDefinitions.IDE0040();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText, []);

        roslynStyleRules.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseStyleRule_IDE0003_0009_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0003-ide0009.md");
        RoslynStyleRule ide0003 = WellKnownRoslynRuleDefinitions.Ide0003();
        RoslynStyleRule ide0009 = WellKnownRoslynRuleDefinitions.Ide0009();

        RoslynStyleRuleGroup roslynStyleRules = _parser.ParseStyleRules(fileText, []);

        roslynStyleRules.Rules.Should().HaveCount(2);
        roslynStyleRules.Rules.ElementAt(0).Should().BeEquivalentTo(ide0003);
        roslynStyleRules.Rules.ElementAt(1).Should().BeEquivalentTo(ide0009);
    }

    private string GetIdeDescription(string fileName)
    {
        string path = Path.Combine(LearnRepositoryPathProvider.GetPathToStyleRules(), fileName);
        string text = File.ReadAllText(path);
        return _preprocessor.Process(text);
    }
}