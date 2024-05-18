using Kysect.Configuin.Common;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnQualityRuleDocumentationParser(LearnMarkdownBlockParser learnMarkdownBlockParser, IMarkdownTextExtractor textExtractor, MarkdownTableParser markdownTableParser, LearnTableParser learnTableParser)
{
    public IReadOnlyCollection<RoslynQualityRule> Parse(string info, IReadOnlyCollection<RoslynQualityRuleOption> roslynQualityRuleOptions)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(textExtractor);

        if (markdownHeadedBlocks.Count == 0)
            throw new ConfiguinException("Style rule markdown file does not contains any heading blocks. Cannot parse description");

        MarkdownHeadedBlock markdownHeadedBlock = markdownHeadedBlocks.First();
        IReadOnlyCollection<Table> contentBlocks = markdownHeadedBlock.Content.OfType<Table>().ToList();
        if (contentBlocks.Count != 1)
            throw new ConfiguinException($"Style rule description block contains unexpected child count. Expected 1, but was {contentBlocks.Count}");

        Table tableBlock = contentBlocks.Single();
        MarkdownTableContent markdownTableContent = markdownTableParser.ParseToSimpleContent(tableBlock);
        LearnPropertyValueDescriptionTable table = learnTableParser.Parse(markdownTableContent);

        LearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        LearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        LearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        // TODO: add this fields to model
        LearnPropertyValueDescriptionTableRow breakingChanges = table.GetSingleValue("Fix is breaking or non-breaking");
        // TODO: remove hardcoded dotnet version
        // TODO: docs contains both .NET7 and .NET8 =_=
        //LearnPropertyValueDescriptionTableRow isDefault = table.GetSingleValue("Enabled by default in .NET 8");

        IReadOnlyCollection<RoslynRuleId> ruleIds = RoslynRuleIdRange.Parse(ruleId.Value).Enumerate().ToList();

        string description = ParseCaRuleDescription(markdownHeadedBlocks);

        var options = roslynQualityRuleOptions
            .Where(o => ruleIds.Any(r => r == o.RuleId))
            .Select(t => t.OptionName)
            .ToList();

        return ruleIds
            .Select(id => new RoslynQualityRule(
                id,
                title.Value,
                category.Value,
                description,
                options))
            .ToList();
    }

    private string ParseCaRuleDescription(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? headedBlock = markdownHeadedBlocks.FirstOrDefault(b => b.HeaderText == "Rule description");
        if (headedBlock is null)
            throw new ConfiguinException("Quality rule page does not contains Rule description block.");

        return learnMarkdownBlockParser.ConvertBlockToText(headedBlock);
    }
}