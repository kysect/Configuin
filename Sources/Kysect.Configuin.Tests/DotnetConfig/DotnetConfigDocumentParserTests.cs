using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.Tests.DotnetConfig.Tools;
using System.Collections.Immutable;

namespace Kysect.Configuin.Tests.DotnetConfig;

public class DotnetConfigDocumentParserTests()
{
    private readonly DotnetConfigDocumentParser _parser = new DotnetConfigDocumentParser();
    private readonly DotnetConfigDocumentComparator _comparator = new DotnetConfigDocumentComparator();

    [Fact]
    public void Parse_EmptyFile_ReturnEmptyDocumentNode()
    {
        const string content = "";
        DotnetConfigDocument expected = new DotnetConfigDocument(ImmutableList<IDotnetConfigSyntaxNode>.Empty, [""]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_TriviaOnly_ReturnEmptyDocumentNode()
    {
        const string content = "# comment";
        DotnetConfigDocument expected = new DotnetConfigDocument(ImmutableList<IDotnetConfigSyntaxNode>.Empty, ["# comment"]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_Category_ReturnDocumentWithCategoryNode()
    {
        const string content = """
                               [*.cs]
                               """;
        DotnetConfigDocument expected = new DotnetConfigDocument([new DotnetConfigCategoryNode("*.cs")]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithComment_ReturnDocumentWithCategoryNode()
    {
        const string content = """
                               [*.cs] # comment
                               """;
        DotnetConfigDocument expected = new DotnetConfigDocument([new DotnetConfigCategoryNode("*.cs") { TrailingTrivia = " comment" }]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_Group_ReturnDocumentWithGroupNode()
    {
        const string content = """
                               ### Custom section ###
                               """;
        DotnetConfigDocument expected = new DotnetConfigDocument([new DotnetConfigSectionNode("### Custom section ###")]);

        DotnetConfigDocument actual = _parser.Parse(content);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void Parse_Property_ReturnDocumentWithPropertyNode()
    {
        const string content = """
                               key=value
                               """;
        DotnetConfigDocument expected = new DotnetConfigDocument([new DotnetConfigRuleOptionNode("key", "value")]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_PropertyWithComment_ReturnDocumentWithPropertyNode()
    {
        const string content = """
                               key=value # comment
                               """;
        DotnetConfigDocument expected = new DotnetConfigDocument([new DotnetConfigRuleOptionNode("key", "value") { TrailingTrivia = " comment" }]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithProperty_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               tab_width=4
                               indent_size=4
                               end_of_line=crlf
                               """;

        DotnetConfigDocument expected = new DotnetConfigDocument([
            new DotnetConfigCategoryNode("*.cs")
                .AddChild(new DotnetConfigGeneralOptionNode("tab_width", "4"))
                .AddChild(new DotnetConfigGeneralOptionNode("indent_size", "4"))
                .AddChild(new DotnetConfigGeneralOptionNode("end_of_line", "crlf"))]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithSectionWithProperty_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               ### Custom section ###
                               tab_width=4
                               indent_size=4
                               end_of_line=crlf
                               """;

        DotnetConfigDocument expected = new DotnetConfigDocument([
            new DotnetConfigCategoryNode("*.cs")
                .AddChild(new DotnetConfigSectionNode("### Custom section ###")
                    .AddChild(new DotnetConfigGeneralOptionNode("tab_width", "4"))
                    .AddChild(new DotnetConfigGeneralOptionNode("indent_size", "4"))
                    .AddChild(new DotnetConfigGeneralOptionNode("end_of_line", "crlf")))
        ]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithPropertyWithLeadingTrivia_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               
                               
                               tab_width=4
                               """;

        DotnetConfigDocument expected = new DotnetConfigDocument([
            new DotnetConfigCategoryNode("*.cs")
                .AddChild(new DotnetConfigGeneralOptionNode("tab_width", "4") { LeadingTrivia = [string.Empty, string.Empty] })
        ]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_SampleDocumentAndSerialize_ReturnSameValue()
    {
        string expected = File.ReadAllText(Path.Combine("Resources", "Editor-config-sample.ini"));
        DotnetConfigDocument document = _parser.Parse(expected);
        string actual = document.ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Parse_DocumentWithAllElements_ReturnSameValue()
    {
        string expected = """
                          # comment
                          [*.cs]
                          ### Category ###
                          key = value
                          """;

        DotnetConfigDocument document = _parser.Parse(expected);
        string actual = document.ToFullString();

        actual.Should().Be(expected);
    }

    private void ParseAndCompare(string content, DotnetConfigDocument expected)
    {
        DotnetConfigDocument actual = _parser.Parse(content);

        _comparator.Compare(actual, expected);
    }
}