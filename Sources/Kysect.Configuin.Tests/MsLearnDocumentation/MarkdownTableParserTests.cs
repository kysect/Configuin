using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParsing.Documents;
using Kysect.Configuin.Core.MarkdownParsing.Tables;
using Kysect.Configuin.Core.MarkdownParsing.Tables.Models;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MarkdownTableParserTests
{
    private readonly MarkdownTableParser _parser = new (PlainTextExtractor.Create());

    [Test]
    public void ParseToSimpleContent_ForSimpleTable_ReturnExpectedResult()
    {
        var input = """
                    |                                     | Value                        |
                    |-------------------------------------|------------------------------|
                    | **Rule ID**                         | CA1000                       |
                    | **Category**                        | [Design](design-warnings.md) |
                    | **Fix is breaking or non-breaking** | Breaking                     |
                    """;
        var expected = new MarkdownTableContent(
            headers: new[] { string.Empty, "Value" },
            rows: new[]
            {
                new[] { "Rule ID", "CA1000" },
                new[] { "Category", "Design" },
                new[] { "Fix is breaking or non-breaking", "Breaking" }
            });

        Table table = ParseToTable(input);
        MarkdownTableContent markdownTableContent = _parser.ParseToSimpleContent(table);

        markdownTableContent.Should().BeEquivalentTo(expected);
    }

    private Table ParseToTable(string content)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(content);
        Table table = markdownDocument.Single().To<Table>();
        return table;
    }
}