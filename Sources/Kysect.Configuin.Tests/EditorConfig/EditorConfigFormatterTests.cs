using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Formatter;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigFormatterTests
{
    private readonly EditorConfigFormatter _formatter;
    private readonly EditorConfigDocumentParser _parser;

    public EditorConfigFormatterTests()
    {
        _formatter = new EditorConfigFormatter(new DotnetConfigSettingsParser(TestLogger.ProviderForTests()));
        _parser = new EditorConfigDocumentParser();
    }

    [Fact]
    public void Format_OrderedIdeRulesWithoutHeader_HeaderAdded()
    {
        var input = """
                    first = value
                    dotnet_diagnostic.IDE0081.severity = none
                    dotnet_diagnostic.IDE0080.severity = none
                    dotnet_diagnostic.IDE0082.severity = warning
                    second = value
                    """;

        var expected = """
                       first = value
                       second = value
                       # Autoformatted values
                       [*.cs]
                       ### IDE ###
                       dotnet_diagnostic.IDE0080.severity = none
                       dotnet_diagnostic.IDE0081.severity = none
                       dotnet_diagnostic.IDE0082.severity = warning
                       """;

        FormatAndCompare(input, expected);
    }

    [Fact]
    public void Format_QualityAndStyleRulesMashed_ReturnOrderedLinesWithHeader()
    {
        var input = """
                    first = value
                    dotnet_diagnostic.IDE0081.severity = none
                    dotnet_diagnostic.CA2001.severity = none
                    second = value
                    dotnet_diagnostic.IDE0080.severity = none
                    dotnet_diagnostic.CA2000.severity = warning
                    """;

        var expected = """
                       first = value
                       second = value
                       # Autoformatted values
                       [*.cs]
                       ### IDE ###
                       dotnet_diagnostic.IDE0080.severity = none
                       dotnet_diagnostic.IDE0081.severity = none
                       ### CA ###
                       dotnet_diagnostic.CA2000.severity = warning
                       dotnet_diagnostic.CA2001.severity = none
                       """;

        FormatAndCompare(input, expected);
    }

    private void FormatAndCompare(string input, string expected)
    {
        EditorConfigDocument editorConfigDocument = _parser.Parse(input);
        EditorConfigDocument formattedDocument = _formatter.Format(editorConfigDocument);
        formattedDocument.ToFullString().Should().Be(expected);
    }
}