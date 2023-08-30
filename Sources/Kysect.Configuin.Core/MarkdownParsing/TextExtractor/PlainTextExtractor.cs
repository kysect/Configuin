using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using System.Text;
using System.Web;

namespace Kysect.Configuin.Core.MarkdownParsing.TextExtractor;

public class PlainTextExtractor : IMarkdownTextExtractor
{
    private readonly MarkdownPipeline _markdownPipeline;

    public static PlainTextExtractor Create()
    {
        return new PlainTextExtractor(MarkdownPipelineProvider.GetDefault());
    }

    public PlainTextExtractor(MarkdownPipeline markdownPipeline)
    {
        _markdownPipeline = markdownPipeline;
    }

    public string ExtractText(Block block)
    {
        var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        var renderer = new HtmlRenderer(streamWriter)
        {
            EnableHtmlForBlock = false,
            EnableHtmlForInline = false,
        };
        _markdownPipeline.Setup(renderer);

        renderer.Render(block);

        streamWriter.Flush();
        string result = Encoding.ASCII
            .GetString(memoryStream.ToArray())
            .Trim();

        // KB: HtmlRenderer change '"' to "&quot;". Decode will change in back
        return HttpUtility
            .HtmlDecode(result)
            // TODO: To smth with this =_=
            .Replace("\r\n", "\n", StringComparison.InvariantCultureIgnoreCase)
            .Replace("\n", Environment.NewLine, StringComparison.InvariantCultureIgnoreCase);
    }
}