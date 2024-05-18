using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Syntax;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnFormattingOptionDocumentationParser(IMarkdownTextExtractor textExtractor, LearnMarkdownBlockParser learnMarkdownBlockParser)
{
    public IReadOnlyCollection<RoslynStyleRuleOption> Parse(LearnDocumentationFileContents fileContents)
    {
        fileContents.ThrowIfNull();

        var result = new List<RoslynStyleRuleOption>();
        IReadOnlyCollection<RoslynStyleRuleOption> dotnetFormattingOptions = ParseAdditionalFormattingOptions(fileContents.DotnetFormattingOptionsContent);
        IReadOnlyCollection<RoslynStyleRuleOption> sharpFormattingOptions = ParseAdditionalFormattingOptions(fileContents.SharpFormattingOptionsContent);

        result.AddRange(dotnetFormattingOptions);
        result.AddRange(sharpFormattingOptions);
        return result;
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> ParseAdditionalFormattingOptions(string dotnetFormattingFileContent)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(dotnetFormattingFileContent);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(textExtractor);
        return learnMarkdownBlockParser.ParseOptions(markdownHeadedBlocks);
    }
}