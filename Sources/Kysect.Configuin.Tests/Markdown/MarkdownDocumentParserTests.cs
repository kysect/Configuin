using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.Tests.Tools;
using Markdig.Syntax;

namespace Kysect.Configuin.Tests.Markdown;

public class MarkdownDocumentParserTests
{
    private readonly IMarkdownTextExtractor _plainTextExtractor = TestImplementations.GetTextExtractor();

    [Fact]
    public void SplitByHeaders_DocumentWithThreeHeaders_ReturnThreeParts()
    {
        string input = """
            ## When to suppress warnings

            It is safe to suppress violations from this rule if the design and performance benefits from implementing the interface are not critical.

            ## Suppress a warning

            If you just want to suppress a single violation, add preprocessor directives to your source file to disable and then re-enable the rule.

            ```csharp
            #pragma warning disable CA1066
            // The code that's violating the rule is on this line.
            #pragma warning restore CA1066
            ```

            To disable the rule for a file, folder, or project, set its severity to `none` in the [configuration file](../configuration-files.md).

            ```ini
            [*.{cs,vb}]
            dotnet_diagnostic.CA1066.severity = none
            ```

            For more information, see [How to suppress code analysis warnings](../suppress-warnings.md).

            ## Related rules

            - [CA1067: Override Equals when implementing IEquatable](ca1067.md)
            """;

        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(input);
        IReadOnlyCollection<MarkdownHeadedBlock> headedBlocks = markdownDocument.SplitByHeaders(_plainTextExtractor);

        headedBlocks.Should().HaveCount(3);
    }
}