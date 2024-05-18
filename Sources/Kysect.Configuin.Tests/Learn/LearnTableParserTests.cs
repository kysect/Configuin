using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Learn.ContentParsing;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Tests.Tools;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Tests.Learn;

public class LearnTableParserTests
{
    private readonly LearnTableParser _learnTableParser = new LearnTableParser();
    private readonly MarkdownTableParser _markdownTableParser = new MarkdownTableParser(TestImplementations.GetTextExtractor());

    [Fact]
    public void Parse_KeyValueTable_ReturnExpectedResult()
    {
        string input = """
                    | Property                            | Value                        |
                    |-------------------------------------|------------------------------|
                    | **Rule ID**                         | CA1000                       |
                    | **Category**                        | [Design](design-warnings.md) |
                    | **Fix is breaking or non-breaking** | Breaking                     |
                    """;

        var expected = new LearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<LearnPropertyValueDescriptionTableRow>>()
            {
                {"Rule ID", new []{new LearnPropertyValueDescriptionTableRow("CA1000") }},
                {"Category", new []{new LearnPropertyValueDescriptionTableRow("Design") }},
                {"Fix is breaking or non-breaking", new []{new LearnPropertyValueDescriptionTableRow("Breaking") }},
            });


        MarkdownTableContent table = ConvertToMarkdownTable(input);
        LearnPropertyValueDescriptionTable learnTableContent = _learnTableParser.Parse(table);

        learnTableContent.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_KeyMultiValueTableContent_ReturnExpectedResult()
    {
        string input = """
                    | Property                 | Value                                                             |
                    | ------------------------ | ----------------------------------------------------------------- |
                    | **Rule ID**              | IDE0058                                                           |
                    | **Title**                | Remove unnecessary expression value                               |
                    | **Category**             | Style                                                             |
                    | **Subcategory**          | Unnecessary code rules                                            |
                    | **Options**              | `csharp_style_unused_value_expression_statement_preference`       |
                    |                          | `visual_basic_style_unused_value_expression_statement_preference` |
                    | **Applicable languages** | C# and Visual Basic                                               |
                    """;

        var expected = new LearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<LearnPropertyValueDescriptionTableRow>>()
            {
                {"Rule ID", new []{new LearnPropertyValueDescriptionTableRow("IDE0058") }},
                {"Title", new []{new LearnPropertyValueDescriptionTableRow("Remove unnecessary expression value") }},
                {"Category", new []{new LearnPropertyValueDescriptionTableRow("Style") }},
                {"Subcategory", new []{new LearnPropertyValueDescriptionTableRow("Unnecessary code rules") }},
                {"Options", new []
                {
                    new LearnPropertyValueDescriptionTableRow("csharp_style_unused_value_expression_statement_preference"),
                    new LearnPropertyValueDescriptionTableRow("visual_basic_style_unused_value_expression_statement_preference"),
                }},
                {"Applicable languages", new []{new LearnPropertyValueDescriptionTableRow("C# and Visual Basic") }}
            });

        MarkdownTableContent table = ConvertToMarkdownTable(input);
        LearnPropertyValueDescriptionTable learnTableContent = _learnTableParser.Parse(table);

        learnTableContent.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_PropertyValueWithDescriptionTable_ReturnExpectedResult()
    {
        string input = """
                    | Property                 | Value                                                            | Description                      |
                    | ------------------------ | ---------------------------------------------------------------- | -------------------------------- |
                    | **Option name**          | dotnet_style_prefer_is_null_check_over_reference_equality_method |                                  |
                    | **Option values**        | `true`                                                           | Prefer `is null` check           |
                    |                          | `false`                                                          | Prefer reference equality method |
                    | **Default option value** | `true`                                                           |                                  |
                    """;

        var expected = new LearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<LearnPropertyValueDescriptionTableRow>>()
            {
                {"Option name", new []{new LearnPropertyValueDescriptionTableRow("dotnet_style_prefer_is_null_check_over_reference_equality_method")}},
                {"Option values", new []
                {
                    new LearnPropertyValueDescriptionTableRow("true", "Prefer is null check"),
                    new LearnPropertyValueDescriptionTableRow("false", "Prefer reference equality method")
                }},
                {"Default option value", new []{new LearnPropertyValueDescriptionTableRow("true") }},
            });

        MarkdownTableContent table = ConvertToMarkdownTable(input);

        LearnPropertyValueDescriptionTable learnTableContent = _learnTableParser.Parse(table);

        learnTableContent.Should().BeEquivalentTo(expected);
    }

    private MarkdownTableContent ConvertToMarkdownTable(string content)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(content);
        Table table = markdownDocument.Single().To<Table>();
        return _markdownTableParser.ParseToSimpleContent(table);
    }
}