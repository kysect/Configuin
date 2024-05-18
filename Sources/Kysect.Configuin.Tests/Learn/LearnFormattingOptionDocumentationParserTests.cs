using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.ContentParsing;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnFormattingOptionDocumentationParserTests
{
    private readonly LearnRepositoryPathProvider _learnRepositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();

    private readonly LearnFormattingOptionDocumentationParser _parser;

    public LearnFormattingOptionDocumentationParserTests()
    {
        IMarkdownTextExtractor markdownTextExtractor = TestImplementations.GetTextExtractor();
        var markdownTableParser = new MarkdownTableParser(markdownTextExtractor);
        var learnTableParser = new LearnTableParser();
        var markdownHeadedBlockParser = new LearnMarkdownBlockParser(markdownTextExtractor, markdownTableParser, learnTableParser);
        _parser = new LearnFormattingOptionDocumentationParser(markdownTextExtractor, markdownHeadedBlockParser);
    }

    [Fact]
    public void Parse_DotnetFormattingOptions_ReturnExpectedResult()
    {
        string pathToDotnetFormattingFile = _learnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string fileContent = File.ReadAllText(pathToDotnetFormattingFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(2);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_sort_system_directives_first());
        roslynStyleRuleOptions.ElementAt(1).Should().BeEquivalentTo(WellKnownRoslynRuleOptionsDefinitions.dotnet_separate_import_directive_groups());
    }

    [Fact]
    public void Parse_CsharpFormattingOptions_ReturnExpectedResult()
    {
        string pathToFile = _learnRepositoryPathProvider.GetPathToSharpFormattingFile();
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
}