using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParser.Tables.Models;
using Kysect.Configuin.Core.MarkdownParser.Tables;
using Kysect.Configuin.Core.MarkdownParser.TextExtractor;
using Kysect.Configuin.Core.MarkdownParser;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables;
using Kysect.Configuin.Tests.MsLearnDocumentation.Asserts;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Syntax;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnTableParserTests
{
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

        var msLearnTableParser = new MsLearnTableParser();
        MarkdownTableContent table = ConvertToMarkdownTable(input);
        MsLearnKeyValueTableContent msLearnTableContent = msLearnTableParser.ParseToKeyValueContent(table);

        Assert.IsNotNull(msLearnTableContent.Headers);

        Assert.That(msLearnTableContent.Headers[0], Is.EqualTo(string.Empty));
        Assert.That(msLearnTableContent.Headers[1], Is.EqualTo("Value"));

        Assert.That(msLearnTableContent.Rows["**Rule ID**"], Is.EqualTo("CA1000"));
        Assert.That(msLearnTableContent.Rows["**Category**"], Is.EqualTo("[Design](design-warnings.md)"));
        Assert.That(msLearnTableContent.Rows["**Fix is breaking or non-breaking**"], Is.EqualTo("Breaking"));
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

        var msLearnTableParser = new MsLearnTableParser();
        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = msLearnTableParser.ToPropertyValueDescriptionTable(table);

        MsLearnPropertyValueDescriptionTableAssert assert = MsLearnPropertyValueDescriptionTableAssert
            .That(msLearnTableContent)
            .RowCountIs(6);

        assert
            .HasProperty("**Subcategory**")
            .WithValue("Unnecessary code rules");

        assert
            .HasProperty("**Options**")
            .WithValue("`csharp_style_unused_value_expression_statement_preference`")
            .WithValue("`visual_basic_style_unused_value_expression_statement_preference`");

        assert
            .HasProperty("**Applicable languages**")
            .WithValue("C# and Visual Basic");
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

        var msLearnTableParser = new MsLearnTableParser();
        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = msLearnTableParser.ToPropertyValueDescriptionTable(table);

        MsLearnPropertyValueDescriptionTableAssert assert = MsLearnPropertyValueDescriptionTableAssert
            .That(msLearnTableContent)
            .RowCountIs(3);

        assert
            .HasProperty("**Option name**")
            .WithValue("dotnet_style_prefer_is_null_check_over_reference_equality_method");

        assert
            .HasProperty("**Option values**")
            .WithValue("`true`", "Prefer `is null` check")
            .WithValue("`false`", "Prefer reference equality method");

        assert
            .HasProperty("**Default option value**")
            .WithValue("`true`");
    }

    private MarkdownTableContent ConvertToMarkdownTable(string content)
    {
        var parser = new MarkdownTableParser(new RoundtripRendererTextExtractor(MarkdownPipelineProvider.GetDefault()));
        MarkdownDocument markdownDocument = MarkdownParser.Parse(content, MarkdownPipelineProvider.GetDefault());
        Table table = markdownDocument.Single().To<Table>();
        return parser.ParseToSimpleContent(table);
    }
}