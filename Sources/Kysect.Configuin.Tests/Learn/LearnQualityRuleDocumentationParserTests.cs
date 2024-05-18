using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.ContentParsing;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnQualityRuleDocumentationParserTests
{
    private readonly LearnRepositoryPathProvider _repositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();
    private readonly LearnQualityRuleDocumentationParser _parser;

    public LearnQualityRuleDocumentationParserTests()
    {
        IMarkdownTextExtractor markdownTextExtractor = TestImplementations.GetTextExtractor();
        var learnTableParser = new LearnTableParser();
        var markdownTableParser = new MarkdownTableParser(markdownTextExtractor);
        var markdownHeadedBlockParser = new LearnMarkdownBlockParser(markdownTextExtractor, markdownTableParser, learnTableParser);
        _parser = new LearnQualityRuleDocumentationParser(markdownHeadedBlockParser, markdownTextExtractor, markdownTableParser, learnTableParser);
    }

    [Fact]
    public void ParseQualityRule_CA1064_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1064.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1064();

        IReadOnlyCollection<RoslynQualityRule> qualityRule = _parser.Parse(fileText, []);

        qualityRule.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseQualityRule_CA1865_CA1867_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1865-ca1867.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1865();

        IReadOnlyCollection<RoslynQualityRule> qualityRules = _parser.Parse(fileText, []);

        qualityRules.Should().HaveCount(3);
        qualityRules.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    private string GetPathToCa(string fileName)
    {
        string path = Path.Combine(_repositoryPathProvider.GetPathToQualityRules(), fileName);
        return File.ReadAllText(path);
    }
}