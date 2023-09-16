using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Markdown.TextExtractor;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Kysect.Configuin.Markdown.Documents;

public static class MarkdownDocumentExtensions
{
    public static MarkdownDocument CreateFromString(string content)
    {
        return MarkdownParser.Parse(content, MarkdownPipelineProvider.GetDefault());
    }

    public static IReadOnlyCollection<MarkdownHeadedBlock> SplitByHeaders(this MarkdownDocument markdownDocument, IMarkdownTextExtractor textExtractor)
    {
        markdownDocument.ThrowIfNull();
        textExtractor.ThrowIfNull();

        var result = new List<MarkdownHeadedBlock>();

        HeadingBlock? headingBlock = null;
        var blocks = new List<Block>();

        foreach (Block block in markdownDocument)
        {
            // TODO: In some case MarkDig return HeadingBlock that is not actually heading. Like CA2153.
            if (block is HeadingBlock currentHeaderBlock && currentHeaderBlock.HeaderChar == '#')
            {
                if (headingBlock is not null)
                {
                    result.Add(new MarkdownHeadedBlock(textExtractor.ExtractText(headingBlock), blocks));
                }

                headingBlock = currentHeaderBlock;
                blocks = new List<Block>();
            }
            else
            {
                blocks.Add(block);
            }
        }

        if (headingBlock is not null)
        {
            result.Add(new MarkdownHeadedBlock(textExtractor.ExtractText(headingBlock), blocks));
        }

        return result;
    }
}