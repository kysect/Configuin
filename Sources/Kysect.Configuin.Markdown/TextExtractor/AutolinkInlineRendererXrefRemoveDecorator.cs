using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Kysect.Configuin.Markdown.TextExtractor;

public class AutolinkInlineRendererXrefRemoveDecorator : HtmlObjectRenderer<AutolinkInline>
{
    private const string XrefPrefix = "xref:";

    private readonly AutolinkInlineRenderer _renderer = new AutolinkInlineRenderer();

    protected override void Write(HtmlRenderer renderer, AutolinkInline obj)
    {
        obj.ThrowIfNull();

        if (obj.Url.StartsWith(XrefPrefix))
            obj.Url = obj.Url.WithoutPrefix(XrefPrefix);

        _renderer.Write(renderer, obj);
    }
}