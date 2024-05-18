using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Tests.Tools;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Tests.Markdown;

public class MarkdownTableParserTests
{
    private readonly MarkdownTableParser _parser = new MarkdownTableParser(TestImplementations.GetTextExtractor());

    [Fact]
    public void ParseToSimpleContent_ForSimpleTable_ReturnExpectedResult()
    {
        string input = """
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