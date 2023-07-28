using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.Documents;

public class MarkdownDocumentParser
{
    private readonly MarkdownPipeline _markdownPipeline;

    public MarkdownDocumentParser(MarkdownPipeline markdownPipeline)
    {
        _markdownPipeline = markdownPipeline;
    }

    public MarkdownDocument Create(string content)
    {
        return MarkdownParser.Parse(content, _markdownPipeline);
    }

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