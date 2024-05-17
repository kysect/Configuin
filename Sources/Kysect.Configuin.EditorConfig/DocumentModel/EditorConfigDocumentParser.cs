using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public class EditorConfigDocumentParser
{
    public EditorConfigDocument Parse(string content)
    {
        content.ThrowIfNull();

        // KB: sometimes Environment.NewLine is not the same as in the file
        string[] lines = content.Split(Environment.NewLine);
        return Parse(lines);
    }

    public EditorConfigDocument Parse(string[] lines)
    {
        lines.ThrowIfNull();

        var context = new EditorConfigDocumentParsingContext();

        foreach (string line in lines)
        {
            var trimmedLine = line.Trim();
            bool isCategory = trimmedLine.StartsWith("[");
            if (isCategory)
            {
                (string? lineWithoutComment, string? comment) = ExtractComment(trimmedLine);
                lineWithoutComment = lineWithoutComment.Trim();
                if (!lineWithoutComment.EndsWith("]"))
                    throw new ArgumentException($"Line is not valid category definition: {lineWithoutComment}");

                string categoryName = lineWithoutComment.Substring(1, lineWithoutComment.Length - 2);

                EditorConfigCategoryNode categoryNode = comment is not null
                    ? new EditorConfigCategoryNode(categoryName) { TrailingTrivia = comment }
                    : new EditorConfigCategoryNode(categoryName);

                context.AddCategory(categoryNode);
                continue;
            }

            bool isSection = trimmedLine.StartsWith(EditorConfigDocumentSectionNode.NodeIndicator);
            if (isSection)
            {
                context.AddSection(trimmedLine);
                continue;
            }

            // TODO: add test for commented properties
            bool isTrivia = trimmedLine.StartsWith("#") || string.IsNullOrWhiteSpace(line);
            if (isTrivia)
            {
                context.AddTrivia(line);
                continue;
            }

            bool isProperty = trimmedLine.Contains('=');
            if (isProperty)
            {
                (string? lineWithoutComment, string? comment) = ExtractComment(trimmedLine);
                lineWithoutComment = lineWithoutComment.Trim();

                string[] parts = lineWithoutComment.Split('=');
                if (parts.Length != 2)
                    throw new ArgumentException($"Line {line} contains unexpected count of '='");

                var propertyNode = new EditorConfigPropertyNode(EditorConfigStringNode.Create(parts[0]), EditorConfigStringNode.Create(parts[1]));
                if (comment is not null)
                    propertyNode = propertyNode with { TrailingTrivia = comment };

                context.AddProperty(propertyNode);
                continue;
            }

            throw new NotSupportedException($"Not supported line: {line}");
        }

        return context.Build();
    }

    private (string lineWithoutComment, string? comment) ExtractComment(string originalString)
    {
        if (!originalString.Contains('#'))
            return (originalString, null);

        string[] parts = originalString.Split('#');
        return (parts[0], parts[1]);
    }
}