using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParser.Documents;

public class MarkdownDocumentParser
{
    public IReadOnlyCollection<MarkdownHeadedBlock> SplitByHeaders(MarkdownDocument markdownDocument)
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