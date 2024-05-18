using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnStyleRuleDocumentationParser(LearnMarkdownBlockParser learnMarkdownBlockParser, MarkdownTableParser markdownTableParser, IMarkdownTextExtractor textExtractor, LearnTableParser learnTableParser)
{
    public RoslynStyleRuleGroup ParseStyleRules(string info, IReadOnlyCollection<RoslynStyleRuleOption> formattingOptions)
    {
        info.ThrowIfNull();
        formattingOptions.ThrowIfNull();

        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(textExtractor);

        if (markdownHeadedBlocks.Count == 0)
            throw new ConfiguinException("Style rule markdown file does not contains any heading blocks. Cannot parse description");

        var ruleDescriptionTables = markdownHeadedBlocks
            .First()
            .Content
            .OfType<Table>()
            .ToList();

        if (ruleDescriptionTables.Count == 0)
            throw new ConfiguinException($"Style rule description block does not contains child table.");

        var roslynStyleRuleInformationTables = ruleDescriptionTables
            .Select(ParseInformationTable)
            .ToList();

        string overviewText = GetStyleOverviewText(markdownHeadedBlocks);
        string? csharpCodeSample = FindIdeExample(markdownHeadedBlocks);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions;
        if (roslynStyleRuleInformationTables.Any(r => r.RuleId == RoslynRuleId.Parse("IDE0055")))
        {
            roslynStyleRuleOptions = formattingOptions;
        }
        else
        {
            roslynStyleRuleOptions = learnMarkdownBlockParser.ParseOptions(markdownHeadedBlocks);
        }

        var rules = roslynStyleRuleInformationTables
            .Select(ConvertToRule)
            .ToList();

        return new RoslynStyleRuleGroup(rules, roslynStyleRuleOptions, overviewText, csharpCodeSample);
    }

    private LearnStyleRuleInformationTable ParseInformationTable(Table tableBlock)
    {
        MarkdownTableContent markdownTableContent = markdownTableParser.ParseToSimpleContent(tableBlock);
        LearnPropertyValueDescriptionTable table = learnTableParser.Parse(markdownTableContent);
        return LearnStyleRuleInformationTable.Create(table);
    }

    private RoslynStyleRule ConvertToRule(LearnStyleRuleInformationTable learnStyleRuleInformationTable)
    {
        return new RoslynStyleRule(
            learnStyleRuleInformationTable.RuleId,
            learnStyleRuleInformationTable.Title,
            learnStyleRuleInformationTable.Category);
    }

    private string GetStyleOverviewText(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? overviewBlock = markdownHeadedBlocks.FirstOrDefault(h => h.HeaderText == "Overview");
        if (overviewBlock is null)
            // TODO: Rule IDE0055 does not contains this block
            //throw new ConfiguinException("Style rule page does not contains Overview block.");
            return string.Empty;

        return learnMarkdownBlockParser.ConvertBlockToText(overviewBlock);
    }

    private string? FindIdeExample(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? exampleBlock = markdownHeadedBlocks.FirstOrDefault(h => h.HeaderText == "Example");
        if (exampleBlock is null)
            return null;

        return learnMarkdownBlockParser.TryExtractCsharpCodeBlock(exampleBlock);
    }
}