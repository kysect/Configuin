using Markdig;
using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.TextExtractor;

public class RoundtripRendererPlainTextExtractor : IMarkdownTextExtractor
{
    private readonly RoundtripRendererTextExtractor _roundtripRendererTextExtractor;

    public RoundtripRendererPlainTextExtractor(MarkdownPipeline markdownPipeline)
    {
        _roundtripRendererTextExtractor = new RoundtripRendererTextExtractor(markdownPipeline);
    }

    public string ExtractText(Block block)
    {
        string result = _roundtripRendererTextExtractor.ExtractText(block);
        return Markdown.ToPlainText(result).Trim();
    }
}