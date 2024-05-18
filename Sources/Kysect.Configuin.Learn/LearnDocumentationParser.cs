using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.Learn.ContentParsing;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.Learn;

public class LearnDocumentationParser : IRoslynRuleDocumentationParser
{
    private readonly ILogger _logger;
    private readonly LearnDocumentationReader _repositoryPathReader;
    private readonly LearnDocumentationPreprocessor _preprocessor;

    private readonly LearnQualityRuleOptionDocumentationParser _qualityRuleOptionDocumentationParser;
    private readonly LearnQualityRuleDocumentationParser _qualityRuleDocumentationParser;
    private readonly LearnFormattingOptionDocumentationParser _formattingOptionDocumentationParser;
    private readonly LearnStyleRuleDocumentationParser _learnStyleRuleDocumentationParser;

    public LearnDocumentationParser(IMarkdownTextExtractor textExtractor, ILogger logger)
    {
        _logger = logger;

        _repositoryPathReader = new LearnDocumentationReader();
        _preprocessor = new LearnDocumentationPreprocessor();
        var markdownTableParser = new MarkdownTableParser(textExtractor);
        var learnTableParser = new LearnTableParser();

        var markdownHeadedBlockParser = new LearnMarkdownBlockParser(textExtractor, markdownTableParser, learnTableParser);
        _qualityRuleOptionDocumentationParser = new LearnQualityRuleOptionDocumentationParser(textExtractor);
        _qualityRuleDocumentationParser = new LearnQualityRuleDocumentationParser(markdownHeadedBlockParser, textExtractor, markdownTableParser, learnTableParser);
        _formattingOptionDocumentationParser = new LearnFormattingOptionDocumentationParser(textExtractor, markdownHeadedBlockParser);
        _learnStyleRuleDocumentationParser = new LearnStyleRuleDocumentationParser(markdownHeadedBlockParser, markdownTableParser, textExtractor, learnTableParser);
    }

    public RoslynRules Parse(string learnRepositoryPath)
    {
        _logger.LogInformation("Parsing roslyn rules from MS Learn");

        learnRepositoryPath.ThrowIfNull();

        LearnDocumentationFileContents learnDocumentationFileContents = _repositoryPathReader.Provide(learnRepositoryPath);
        learnDocumentationFileContents = _preprocessor.Process(learnDocumentationFileContents);

        IReadOnlyCollection<RoslynQualityRuleOption> roslynQualityRuleOptions = _qualityRuleOptionDocumentationParser.Parse(learnDocumentationFileContents.QualityRuleOptionFileContent);
        IReadOnlyCollection<RoslynStyleRuleOption> formattingOptions = _formattingOptionDocumentationParser.Parse(learnDocumentationFileContents);
        List<RoslynQualityRule> roslynQualityRules = learnDocumentationFileContents.QualityRuleFileContents.SelectMany(s => _qualityRuleDocumentationParser.Parse(s, roslynQualityRuleOptions)).ToList();
        List<RoslynStyleRuleGroup> roslynStyleRules = learnDocumentationFileContents.StyleRuleFileContents.Select(s => _learnStyleRuleDocumentationParser.ParseStyleRules(s, formattingOptions)).ToList();
        roslynStyleRules = AddNamingRule(roslynStyleRules);

        return new RoslynRules(roslynQualityRules, roslynStyleRules);
    }

    private List<RoslynStyleRuleGroup> AddNamingRule(List<RoslynStyleRuleGroup> roslynStyleRules)
    {
        var namingRule = new RoslynStyleRule(RoslynNameRuleInfo.RuleId, "Code-style naming rules", "Style");
        var roslynStyleRuleGroup = new RoslynStyleRuleGroup(namingRule, string.Empty, null);

        roslynStyleRules.Add(roslynStyleRuleGroup);
        return roslynStyleRules;
    }
}