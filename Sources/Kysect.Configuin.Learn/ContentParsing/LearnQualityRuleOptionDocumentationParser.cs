using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnQualityRuleOptionDocumentationParser(IMarkdownTextExtractor textExtractor)
{
    public IReadOnlyCollection<RoslynQualityRuleOption> Parse(string content)
    {
        content.ThrowIfNull();

        string qualityRuleOptionFileContent = content;

        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(qualityRuleOptionFileContent);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(textExtractor);

        MarkdownHeadedBlock optionListBlock = markdownHeadedBlocks.Single(b => b.HeaderText == "Options");
        ListBlock optionList = optionListBlock
            .Content
            .OfType<ListBlock>()
            .Single();

        var options = new List<string>();

        foreach (Block optionListLine in optionList)
        {

            if (optionListLine is not ListItemBlock listItemBlock)
                throw new ConfiguinException("Unexpected block in options list");

            if (listItemBlock.Single() is not ParagraphBlock paragraphBlock)
                throw new ConfiguinException("Unexpected block in options list");

            paragraphBlock.Inline.ThrowIfNull();
            LinkInline linkInline = paragraphBlock.Inline.OfType<LinkInline>().Single();
            linkInline.Url.ThrowIfNull();
            // TODO: some kind of hack
            string optionName = linkInline.Url.Replace("#", "");
            options.Add(optionName);
        }

        var result = new List<RoslynQualityRuleOption>();
        foreach (string option in options)
        {
            MarkdownHeadedBlock optionBlock = markdownHeadedBlocks.Single(b => b.HeaderText == option);
            RoslynQualityRuleOption? roslynQualityRuleOption = TryParseQualityRuleOption(optionBlock);

            if (roslynQualityRuleOption is null)
                continue;
            result.Add(roslynQualityRuleOption);
        }

        return result;
    }

    private RoslynQualityRuleOption? TryParseQualityRuleOption(MarkdownHeadedBlock block)
    {
        Table table = block
            .Content
            .OfType<Table>()
            .First();

        if (table.Count != 2)
            throw new ConfiguinException("Quality rule option table does not contains 2 rows");

        TableRow valueRow = table.Last().To<TableRow>();
        if (valueRow.Count != 4)
            throw new ConfiguinException("Quality rule option table does not contains 4 columns");

        var usedIdRules = textExtractor.ExtractText(valueRow[3]).Split();
        // TODO: support case when multiple rules are used
        if (usedIdRules.Length != 1)
            return null;

        return new RoslynQualityRuleOption(RoslynRuleId.Parse(usedIdRules[0]), block.HeaderText);
    }
}