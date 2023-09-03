using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParsing.Documents;
using Kysect.Configuin.Core.MarkdownParsing.Tables;
using Kysect.Configuin.Core.MarkdownParsing.Tables.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using NUnit.Framework;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnTableParserTests
{
    private readonly MsLearnTableParser _msLearnTableParser = new MsLearnTableParser();
    private readonly MarkdownTableParser _markdownTableParser = new MarkdownTableParser(TestImplementations.GetTextExtractor());

    [Test]
    public void Parse_KeyValueTable_ReturnExpectedResult()
    {
        string input = """
                    |                                     | Value                        |
                    |-------------------------------------|------------------------------|
                    | **Rule ID**                         | CA1000                       |
                    | **Category**                        | [Design](design-warnings.md) |
                    | **Fix is breaking or non-breaking** | Breaking                     |
                    """;

        var expected = new MsLearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>>()
            {
                {"Rule ID", new []{new MsLearnPropertyValueDescriptionTableRow("CA1000") }},
                {"Category", new []{new MsLearnPropertyValueDescriptionTableRow("Design") }},
                {"Fix is breaking or non-breaking", new []{new MsLearnPropertyValueDescriptionTableRow("Breaking") }},
            });


        MarkdownTableContent table = ConvertToMarkdownTable(input);
        MsLearnPropertyValueDescriptionTable msLearnTableContent = _msLearnTableParser.Parse(table);

        msLearnTableContent.Should().BeEquivalentTo(expected);
    }

    [Test]
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

        var expected = new MsLearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>>()
            {
                {"Rule ID", new []{new MsLearnPropertyValueDescriptionTableRow("IDE0058") }},
                {"Title", new []{new MsLearnPropertyValueDescriptionTableRow("Remove unnecessary expression value") }},
                {"Category", new []{new MsLearnPropertyValueDescriptionTableRow("Style") }},
                {"Subcategory", new []{new MsLearnPropertyValueDescriptionTableRow("Unnecessary code rules") }},
                {"Options", new []
                {
                    new MsLearnPropertyValueDescriptionTableRow("csharp_style_unused_value_expression_statement_preference"),
                    new MsLearnPropertyValueDescriptionTableRow("visual_basic_style_unused_value_expression_statement_preference"),
                }},
                {"Applicable languages", new []{new MsLearnPropertyValueDescriptionTableRow("C# and Visual Basic") }}
            });

        MarkdownTableContent table = ConvertToMarkdownTable(input);
        MsLearnPropertyValueDescriptionTable msLearnTableContent = _msLearnTableParser.Parse(table);

        msLearnTableContent.Should().BeEquivalentTo(expected);
    }

    [Test]
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

        var expected = new MsLearnPropertyValueDescriptionTable(
            new Dictionary<string, IReadOnlyList<MsLearnPropertyValueDescriptionTableRow>>()
            {
                {"Option name", new []{new MsLearnPropertyValueDescriptionTableRow("dotnet_style_prefer_is_null_check_over_reference_equality_method")}},
                {"Option values", new []
                {
                    new MsLearnPropertyValueDescriptionTableRow("true", "Prefer is null check"),
                    new MsLearnPropertyValueDescriptionTableRow("false", "Prefer reference equality method")
                }},
                {"Default option value", new []{new MsLearnPropertyValueDescriptionTableRow("true") }},
            });

        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = _msLearnTableParser.Parse(table);

        msLearnTableContent.Should().BeEquivalentTo(expected);
    }

    private MarkdownTableContent ConvertToMarkdownTable(string content)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(content);
        Table table = markdownDocument.Single().To<Table>();
        return _markdownTableParser.ParseToSimpleContent(table);
    }
}