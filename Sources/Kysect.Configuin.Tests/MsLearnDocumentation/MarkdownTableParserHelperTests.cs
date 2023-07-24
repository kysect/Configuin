using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParser;
using Kysect.Configuin.Core.MarkdownParser.Tables.Models;
using Kysect.Configuin.Core.MarkdownParser.TextExtractor;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Syntax;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MarkdownTableParserHelperTests
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

    [Test]
    public void ParseTable_ToKeyValueTableContent_ReturnExpectedResult()
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
        MarkdownKeyValueTableContent markdownTableContent = parser.ParseToKeyValueContent(table);

        Assert.IsNotNull(markdownTableContent.Headers);

        Assert.That(markdownTableContent.Headers[0], Is.EqualTo(string.Empty));
        Assert.That(markdownTableContent.Headers[1], Is.EqualTo("Value"));

        Assert.That(markdownTableContent.Rows["**Rule ID**"], Is.EqualTo("CA1000"));
        Assert.That(markdownTableContent.Rows["**Category**"], Is.EqualTo("[Design](design-warnings.md)"));
        Assert.That(markdownTableContent.Rows["**Fix is breaking or non-breaking**"], Is.EqualTo("Breaking"));
    }

    // TODO: Need to implement removing '*' on text extracting
    [Test]
    [Ignore("TODO: Need to implement")]
    public void ParseTable_ToKeyValueTableContent_ReturnExpectedResultWithoutFormattingChars()
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
        MarkdownKeyValueTableContent markdownTableContent = parser.ParseToKeyValueContent(table);

        Assert.IsNotNull(markdownTableContent.Headers);

        Assert.That(markdownTableContent.Headers[0], Is.EqualTo(string.Empty));
        Assert.That(markdownTableContent.Headers[1], Is.EqualTo("Value"));

        Assert.That(markdownTableContent.Rows["Rule ID"], Is.EqualTo("CA1000"));
        Assert.That(markdownTableContent.Rows["Category"], Is.EqualTo("Design"));
        Assert.That(markdownTableContent.Rows["Fix is breaking or non-breaking"], Is.EqualTo("Breaking"));
    }

    [Test]
    public void ParseTable_ToKeyMultiValueTableContent_ReturnExpectedResult()
    {
        string input = @"
| Property                 | Value                                                             |
| ------------------------ | ----------------------------------------------------------------- |
| **Rule ID**              | IDE0058                                                           |
| **Title**                | Remove unnecessary expression value                               |
| **Category**             | Style                                                             |
| **Subcategory**          | Unnecessary code rules                                            |
| **Options**              | `csharp_style_unused_value_expression_statement_preference`       |
|                          | `visual_basic_style_unused_value_expression_statement_preference` |
| **Applicable languages** | C# and Visual Basic                                               |
";

        MarkdownTableParser parser = CreateParser();

        Table table = ParseToTable(input);
        MarkdownKeyMultiValueTableContent markdownTableContent = parser.ParseToMultiValueTable(table);

        Assert.IsNotNull(markdownTableContent.Headers);
        Assert.That(markdownTableContent.Rows.Count, Is.EqualTo(6));
        Assert.That(markdownTableContent.Rows["**Subcategory**"], Is.EqualTo(new[] { "Unnecessary code rules" }));
        Assert.That(markdownTableContent.Rows["**Options**"], Is.EqualTo(new[] { "`csharp_style_unused_value_expression_statement_preference`", "`visual_basic_style_unused_value_expression_statement_preference`" }));
        Assert.That(markdownTableContent.Rows["**Applicable languages**"], Is.EqualTo(new[] { "C# and Visual Basic" }));
    }

    [Test]
    public void ParseTable_ToPropertyValueWithDescriptionTable_ReturnExpectedResult()
    {
        string input = @"
| Property                 | Value                                                            | Description                      |
| ------------------------ | ---------------------------------------------------------------- | -------------------------------- |
| **Option name**          | dotnet_style_prefer_is_null_check_over_reference_equality_method |                                  |
| **Option values**        | `true`                                                           | Prefer `is null` check           |
|                          | `false`                                                          | Prefer reference equality method |
| **Default option value** | `true`                                                           |                                  |";

        MarkdownTableParser parser = CreateParser();

        Table table = ParseToTable(input);
        MarkdownPropertyValueWithDescriptionTable markdownTableContent = parser.ParsePropertyValueWithDescriptionTable(table);

        Assert.That(markdownTableContent.Rows.Count, Is.EqualTo(3));
        Assert.That(markdownTableContent.Rows["**Option name**"],
            Is.EqualTo(new[]
            {
                new MarkdownPropertyValueWithDescriptionTableRow("dotnet_style_prefer_is_null_check_over_reference_equality_method", string.Empty)
            }));

        Assert.That(markdownTableContent.Rows["**Option values**"],
            Is.EqualTo(new[]
            {
                new MarkdownPropertyValueWithDescriptionTableRow("`true`", "Prefer `is null` check"),
                new MarkdownPropertyValueWithDescriptionTableRow("`false`", "Prefer reference equality method")
            }));

        Assert.That(markdownTableContent.Rows["**Default option value**"],
            Is.EqualTo(new[]
            {
                new MarkdownPropertyValueWithDescriptionTableRow("`true`", string.Empty)
            }));
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