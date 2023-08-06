using Markdig;
using Markdig.Renderers.Roundtrip;
using Markdig.Syntax;
using System.Text;

namespace Kysect.Configuin.Core.MarkdownParsing.TextExtractor;

public class RoundtripRendererPlainTextExtractor : IMarkdownTextExtractor
{
    private readonly MarkdownPipeline _markdownPipeline;

    public RoundtripRendererPlainTextExtractor(MarkdownPipeline markdownPipeline)
    {
        _markdownPipeline = markdownPipeline;
    }

    public string ExtractText(Block block)
    {
        var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        var renderer = new RoundtripRenderer(streamWriter);
        _markdownPipeline.Setup(renderer);

        renderer.Render(block);

        streamWriter.Flush();
        string result = Encoding.ASCII.GetString(memoryStream.ToArray());
        return Markdown.ToPlainText(result).Trim();
    }
}