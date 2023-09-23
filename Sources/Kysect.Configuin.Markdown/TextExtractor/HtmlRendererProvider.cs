using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;

namespace Kysect.Configuin.Markdown.TextExtractor;

public static class HtmlRendererProvider
{
    public static HtmlRenderer Create(StreamWriter streamWriter)
    {
        var renderer = new HtmlRenderer(streamWriter)
        {
            EnableHtmlForBlock = false,
            EnableHtmlForInline = false,
        };

        renderer.ObjectRenderers.Replace<AutolinkInlineRenderer>(new AutolinkInlineRendererXrefRemoveDecorator());

        return renderer;
    }
}