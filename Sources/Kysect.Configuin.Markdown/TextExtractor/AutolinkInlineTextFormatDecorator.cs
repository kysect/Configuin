using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Kysect.Configuin.Markdown.TextExtractor;

public class AutolinkInlineTextFormatDecorator : HtmlObjectRenderer<AutolinkInline>
{
    private const string XrefPrefix = "xref:";

    private readonly AutolinkInlineRenderer _renderer = new AutolinkInlineRenderer();

    protected override void Write(HtmlRenderer renderer, AutolinkInline obj)
    {
        obj.ThrowIfNull();

        if (obj.Url.StartsWith(XrefPrefix))
            obj.Url = obj.Url.WithoutPrefix(XrefPrefix);

        var postfixes = new string[]
        {
            "%2A?displayProperty=nameWithType",
            "%2A?displayProperty=fullName",
            "?displayProperty=fullName",
            "?displayProperty=nameWithType"
        };

        foreach (string postfix in postfixes)
        {
            if (obj.Url.EndsWith(postfix))
                obj.Url = obj.Url[..^postfix.Length];
        }

        _renderer.Write(renderer, obj);
    }
}