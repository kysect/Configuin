using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.Documents;

public class MarkdownHeadedBlock
{
    public HeadingBlock Header { get; }
    public IReadOnlyCollection<Block> Content { get; }

    public MarkdownHeadedBlock(HeadingBlock header, IReadOnlyCollection<Block> content)
    {
        Header = header;
        Content = content;
    }

    // TODO: remove this method and pass header text instead of header block
    public string GetHeaderText()
    {
        return PlainTextExtractor.Create().ExtractText(Header);
    }
}