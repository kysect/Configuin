using FluentAssertions;
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
    private MarkdownTableParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MarkdownTableParser(new RoundtripRendererTextExtractor(MarkdownPipelineProvider.GetDefault()));
    }

    [Test]
    public void ParseToSimpleContent_ForSimpleTable_ReturnExpectedResult()
    {
        var input = @"
|                                     | Value                        |
|-------------------------------------|------------------------------|
| **Rule ID**                         | CA1000                       |
| **Category**                        | [Design](design-warnings.md) |
| **Fix is breaking or non-breaking** | Breaking                     |
";

        Table table = ParseToTable(input);

        MarkdownTableContent markdownTableContent = _parser.ParseToSimpleContent(table);

        markdownTableContent.Headers
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.Equal(string.Empty, "Value");

        markdownTableContent.Rows
            .Should().HaveCount(3);

        markdownTableContent.Rows[0]
            .Should().Equal("**Rule ID**", "CA1000");

        markdownTableContent.Rows[1]
            .Should().Equal("**Category**", "[Design](design-warnings.md)");

        markdownTableContent.Rows[2]
            .Should().Equal("**Fix is breaking or non-breaking**", "Breaking");
    }

    private Table ParseToTable(string content)
    {
        MarkdownDocument markdownDocument = MarkdownParser.Parse(content, MarkdownPipelineProvider.GetDefault());
        Table table = markdownDocument.Single().To<Table>();
        return table;
    }
}