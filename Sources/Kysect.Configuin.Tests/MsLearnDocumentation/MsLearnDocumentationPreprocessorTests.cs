using Kysect.Configuin.MsLearn;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnDocumentationPreprocessorTests
{
    private readonly MsLearnDocumentationPreprocessor _preprocessor = new MsLearnDocumentationPreprocessor();

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