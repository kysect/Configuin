using Kysect.Configuin.Learn.ContentParsing;

namespace Kysect.Configuin.Tests.Learn;

public class LearnDocumentationPreprocessorTests
{
    private readonly LearnDocumentationPreprocessor _preprocessor = new LearnDocumentationPreprocessor();

    [Fact]
    public void Process_InputWithZones_RemoveOnlyFsharpBlock()
    {
        var input = """
                    first line
                    :::zone pivot="lang-csharp-vb"
                    second line
                    :::zone-end
                    third line
                    :::zone pivot="lang-fsharp"
                    fourth line
                    :::zone-end
                    fifth line
                    """;

        var expected = """
                       first line
                       second line
                       third line
                       fifth line
                       """;

        string actual = _preprocessor.Process(input);

        actual.Should().Be(expected);
    }
}