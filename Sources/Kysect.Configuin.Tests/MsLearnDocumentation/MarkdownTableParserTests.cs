using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParser;
using Kysect.Configuin.Core.MarkdownParser.Tables;
using Kysect.Configuin.Core.MarkdownParser.Tables.Models;
using Kysect.Configuin.Core.MarkdownParser.TextExtractor;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Syntax;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MarkdownTableParserTests
{
    [Test]
    public void ParseTable_ToSimpleTableContent_ReturnExpectedResult()
    {
        string input = @"
|                                     | Value                        |
|-------------------------------------|------------------------------|
| **Rule ID**                         | CA1000                       |
| **Category**                        | [Design](design-warnings.md) |
| **Fix is breaking or non-breaking** | Breaking                     |
";

        MarkdownTableParser parser = CreateParser();

        Table table = ParseToTable(input);
        MarkdownTableContent markdownTableContent = parser.ParseToSimpleContent(table);

        Assert.IsNotNull(markdownTableContent.Headers);

        Assert.That(markdownTableContent.Headers[0], Is.EqualTo(string.Empty));
        Assert.That(markdownTableContent.Headers[1], Is.EqualTo("Value"));

        Assert.That(markdownTableContent.Rows[0][0], Is.EqualTo("**Rule ID**"));
        Assert.That(markdownTableContent.Rows[0][1], Is.EqualTo("CA1000"));
        Assert.That(markdownTableContent.Rows[1][0], Is.EqualTo("**Category**"));
        Assert.That(markdownTableContent.Rows[1][1], Is.EqualTo("[Design](design-warnings.md)"));
        Assert.That(markdownTableContent.Rows[2][0], Is.EqualTo("**Fix is breaking or non-breaking**"));
        Assert.That(markdownTableContent.Rows[2][1], Is.EqualTo("Breaking"));
    }

    private MarkdownTableParser CreateParser()
    {
        return new MarkdownTableParser(new RoundtripRendererTextExtractor(MarkdownPipelineProvider.GetDefault()));
    }

    private Table ParseToTable(string content)
    {
        MarkdownDocument markdownDocument = MarkdownParser.Parse(content, MarkdownPipelineProvider.GetDefault());
        Table table = markdownDocument.Single().To<Table>();
        return table;
    }
}