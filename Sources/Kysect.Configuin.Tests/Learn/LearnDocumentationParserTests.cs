using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnDocumentationParserTests
{
    private readonly IRoslynRuleDocumentationParser _roslynRuleDocumentationParser;

    public LearnDocumentationParserTests()
    {
        IMarkdownTextExtractor markdownTextExtractor = TestImplementations.GetTextExtractor();
        _roslynRuleDocumentationParser = new LearnDocumentationParser(markdownTextExtractor, TestLogger.ProviderForTests());
    }

    [Fact]
    public void Parse_MsDocsRepository_FinishWithoutError()
    {
        RoslynRules roslynRules = _roslynRuleDocumentationParser.Parse(Constants.GetPathToMsDocsRoot());

        roslynRules.QualityRules.Single(r => r.RuleId.ToString() == "CA2007").Options.Should().HaveCount(2);
        // TODO: add asserts
    }
}