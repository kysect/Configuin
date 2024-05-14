using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.Tests.EditorConfig.Tools;
using System.Collections.Immutable;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigDocumentParserTests()
{
    private readonly EditorConfigDocumentParser _parser = new EditorConfigDocumentParser();
    private readonly EditorConfigDocumentComparator _comparator = new EditorConfigDocumentComparator();

    [Fact]
    public void Parse_EmptyFile_ReturnEmptyDocumentNode()
    {
        const string content = "";
        EditorConfigDocument expected = new EditorConfigDocument(ImmutableList<IEditorConfigNode>.Empty);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_Category_ReturnDocumentWithCategoryNode()
    {
        const string content = """
                               [*.cs]
                               """;
        EditorConfigDocument expected = new EditorConfigDocument([new EditorConfigCategoryNode("*.cs")]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_Group_ReturnDocumentWithGroupNode()
    {
        const string content = """
                               ### Custom section ###
                               """;
        EditorConfigDocument expected = new EditorConfigDocument([new EditorConfigDocumentSectionNode("Custom section")]);

        EditorConfigDocument actual = _parser.Parse(content);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void Parse_Property_ReturnDocumentWithPropertyNode()
    {
        const string content = """
                               key = value
                               """;
        EditorConfigDocument expected = new EditorConfigDocument([new EditorConfigPropertyNode("key", "value")]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithProperty_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               tab_width = 4
                               indent_size = 4
                               end_of_line = crlf
                               """;

        EditorConfigDocument expected = new EditorConfigDocument([
            new EditorConfigCategoryNode("*.cs")
                .AddChild(new EditorConfigPropertyNode("tab_width", "4"))
                .AddChild(new EditorConfigPropertyNode("indent_size", "4"))
                .AddChild(new EditorConfigPropertyNode("end_of_line", "crlf"))]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithSectionWithProperty_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               ### Custom section ###
                               tab_width = 4
                               indent_size = 4
                               end_of_line = crlf
                               """;

        EditorConfigDocument expected = new EditorConfigDocument([
            new EditorConfigCategoryNode("*.cs")
                .AddChild(new EditorConfigDocumentSectionNode("Custom section")
                    .AddChild(new EditorConfigPropertyNode("tab_width", "4"))
                    .AddChild(new EditorConfigPropertyNode("indent_size", "4"))
                    .AddChild(new EditorConfigPropertyNode("end_of_line", "crlf")))
        ]);

        ParseAndCompare(content, expected);
    }

    [Fact]
    public void Parse_CategoryWithPropertyWithLeadingTrivia_ReturnCorrectDocument()
    {
        const string content = """
                               [*.cs]
                               
                               
                               tab_width = 4
                               """;

        EditorConfigDocument expected = new EditorConfigDocument([
            new EditorConfigCategoryNode("*.cs")
                .AddChild(new EditorConfigPropertyNode("tab_width", "4") { LeadingTrivia = [string.Empty, string.Empty] })
        ]);

        ParseAndCompare(content, expected);
    }

    private void ParseAndCompare(string content, EditorConfigDocument expected)
    {
        EditorConfigDocument actual = _parser.Parse(content);

        _comparator.Compare(actual, expected);
    }
}