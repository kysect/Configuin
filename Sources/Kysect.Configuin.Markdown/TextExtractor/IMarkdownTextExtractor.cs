using Markdig.Syntax;

namespace Kysect.Configuin.Markdown.TextExtractor;

public interface IMarkdownTextExtractor
{
    string ExtractText(Block block);
}