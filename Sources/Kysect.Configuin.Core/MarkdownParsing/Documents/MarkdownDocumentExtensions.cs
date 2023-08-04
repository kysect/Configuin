using Markdig.Parsers;
using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.Documents;

public static class MarkdownDocumentExtensions
{
    public static MarkdownDocument CreateFromString(string content)
    {
        return MarkdownParser.Parse(content, MarkdownPipelineProvider.GetDefault());
    }

    public static IReadOnlyCollection<MarkdownHeadedBlock> SplitByHeaders(this MarkdownDocument markdownDocument)
    {
        ArgumentNullException.ThrowIfNull(markdownDocument);

        var result = new List<MarkdownHeadedBlock>();

        HeadingBlock? headingBlock = null;
        var blocks = new List<Block>();

        foreach (Block block in markdownDocument)
        {
            if (block is HeadingBlock currentHeaderBlock)
            {
                if (headingBlock is not null)
                {
                    result.Add(new MarkdownHeadedBlock(headingBlock, blocks));
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
            result.Add(new MarkdownHeadedBlock(headingBlock, blocks));
        }

        return result;
    }
}