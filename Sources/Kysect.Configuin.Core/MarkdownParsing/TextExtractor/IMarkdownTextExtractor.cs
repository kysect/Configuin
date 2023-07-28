using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.TextExtractor;

public interface IMarkdownTextExtractor
{
    string ExtractText(Block block);
}