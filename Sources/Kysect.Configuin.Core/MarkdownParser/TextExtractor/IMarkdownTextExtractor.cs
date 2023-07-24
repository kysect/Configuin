using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParser.TextExtractor;

public interface IMarkdownTextExtractor
{
    string ExtractText(Block block);
}