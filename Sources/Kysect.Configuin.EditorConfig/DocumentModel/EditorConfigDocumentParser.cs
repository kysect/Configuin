using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public class EditorConfigDocumentParser
{
    public EditorConfigDocument Parse(string content)
    {
        content.ThrowIfNull();

        string[] lines = content.Split(Environment.NewLine);

        var context = new EditorConfigDocumentParsingContext();
        //List<string> currentTrivia = new List<string>();

        foreach (string line in lines)
        {
            var trimmedLine = line.Trim();
            bool isCategory = trimmedLine.StartsWith("[");
            if (isCategory)
            {
                var categoryName = line.Substring(1, line.Length - 2);
                context.AddCategory(categoryName);
                continue;
            }

            bool isSection = trimmedLine.StartsWith(EditorConfigDocumentSectionNode.NodeIndicator);
            if (isSection)
            {
                int indicatorLength = EditorConfigDocumentSectionNode.NodeIndicator.Length;
                string sectionTitle = line.Substring(indicatorLength, line.Length - (indicatorLength * 2)).Trim();
                context.AddSection(sectionTitle);
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
                string[] parts = line.Split('=');
                if (parts.Length != 2)
                    throw new ArgumentException($"Line {line} contains unexpected count of '='");

                string key = parts[0].Trim();
                string value = parts[1].Trim();
                EditorConfigPropertyNode propertyNode = new EditorConfigPropertyNode(key, value);
                context.AddProperty(propertyNode);
                continue;
            }

            throw new NotSupportedException($"Not supported line: {line}");
        }

        return context.Build();
    }
}