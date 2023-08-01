using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.MarkdownParsing;
using Kysect.Configuin.Core.MarkdownParsing.Documents;
using Kysect.Configuin.Core.MarkdownParsing.Tables;
using Kysect.Configuin.Core.MarkdownParsing.Tables.Models;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables;
using Kysect.Configuin.Tests.MsLearnDocumentation.Asserts;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnTableParserTests
{
    private MsLearnTableParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MsLearnTableParser();
    }

    [Test]
    public void Parse_KeyValueTable_ReturnExpectedResult()
    {
        var input = """
                    |                                     | Value                        |
                    |-------------------------------------|------------------------------|
                    | **Rule ID**                         | CA1000                       |
                    | **Category**                        | [Design](design-warnings.md) |
                    | **Fix is breaking or non-breaking** | Breaking                     |
                    """;

        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = _parser.Parse(table);

        msLearnTableContent.Properties
            .Should().HaveCount(3);

        msLearnTableContent.Properties
            .Should().ContainKey("**Rule ID**")
            .WhoseValue.Should().Contain("CA1000");

        msLearnTableContent.Properties
            .Should().ContainKey("**Category**")
            .WhoseValue.Should().Contain("[Design](design-warnings.md)");

        msLearnTableContent.Properties
            .Should().ContainKey("**Fix is breaking or non-breaking**")
            .WhoseValue.Should().Contain("Breaking");
    }

    [Test]
    public void Parse_KeyMultiValueTableContent_ReturnExpectedResult()
    {
        var input = """
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

        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = _parser.Parse(table);

        msLearnTableContent.Properties
            .Should().HaveCount(6);

        msLearnTableContent.Properties
            .Should().ContainKey("**Subcategory**")
            .WhoseValue.Should().Contain("Unnecessary code rules");

        msLearnTableContent.Properties
            .Should().ContainKey("**Options**")
            .WhoseValue.Should().Contain("`csharp_style_unused_value_expression_statement_preference`")
            .And.Contain("`visual_basic_style_unused_value_expression_statement_preference`");

        msLearnTableContent.Properties
            .Should().ContainKey("**Applicable languages**")
            .WhoseValue.Should().Contain("C# and Visual Basic");
    }

    [Test]
    public void Parse_PropertyValueWithDescriptionTable_ReturnExpectedResult()
    {
        var input = """
                    | Property                 | Value                                                            | Description                      |
                    | ------------------------ | ---------------------------------------------------------------- | -------------------------------- |
                    | **Option name**          | dotnet_style_prefer_is_null_check_over_reference_equality_method |                                  |
                    | **Option values**        | `true`                                                           | Prefer `is null` check           |
                    |                          | `false`                                                          | Prefer reference equality method |
                    | **Default option value** | `true`                                                           |                                  |
                    """;

        MarkdownTableContent table = ConvertToMarkdownTable(input);

        MsLearnPropertyValueDescriptionTable msLearnTableContent = _parser.Parse(table);

        msLearnTableContent.Properties
            .Should().HaveCount(3);

        msLearnTableContent.Properties
            .Should().ContainKey("**Option name**")
            .WhoseValue.Should().Contain("dotnet_style_prefer_is_null_check_over_reference_equality_method");

        msLearnTableContent.Properties
            .Should().ContainKey("**Option values**")
            .WhoseValue.Should().Contain("`true`", "Prefer `is null` check")
            .And.Contain("`false`", "Prefer reference equality method");

        msLearnTableContent.Properties
            .Should().ContainKey("**Default option value**")
            .WhoseValue.Should().Contain("`true`");
    }

    private MarkdownTableContent ConvertToMarkdownTable(string content)
    {
        var parser = new MarkdownTableParser(new RoundtripRendererTextExtractor(MarkdownPipelineProvider.GetDefault()));
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(content);
        Table table = markdownDocument.Single().To<Table>();
        return parser.ParseToSimpleContent(table);
    }
}