using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.DotnetConfig.Syntax;

public class DotnetConfigDocumentParser
{
    private readonly HashSet<string> _generalRuleKeys;

    public DotnetConfigDocumentParser()
    {
        // TODO: Investigate other rules
        _generalRuleKeys = new HashSet<string>
        {
            "tab_width",
            "indent_size",
            "end_of_line"
        };
    }

    public DotnetConfigDocument Parse(string content)
    {
        content.ThrowIfNull();

        // KB: sometimes Environment.NewLine is not the same as in the file
        string[] lines = content.Split(Environment.NewLine);
        return Parse(lines);
    }

    public DotnetConfigDocument Parse(string[] lines)
    {
        lines.ThrowIfNull();

        var context = new DotnetConfigDocumentParsingContext();

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

                DotnetConfigCategoryNode categoryNode = comment is not null
                    ? new DotnetConfigCategoryNode(categoryName) { TrailingTrivia = comment }
                    : new DotnetConfigCategoryNode(categoryName);

                context.AddCategory(categoryNode);
                continue;
            }

            bool isSection = trimmedLine.StartsWith(DotnetConfigSectionNode.NodeIndicator);
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

                var keyNode = DotnetConfigStringNode.Create(parts[0]);
                var valueNode = DotnetConfigStringNode.Create(parts[1]);
                EqualSymbolSyntaxNode equalSymbol = new EqualSymbolSyntaxNode(keyNode.TrailingTrivia, valueNode.LeadingTrivia);

                IDotnetConfigPropertySyntaxNode dotnetConfigSyntaxSetting = ParseSetting(keyNode.Value, equalSymbol, valueNode.Value);

                if (comment is not null)
                    dotnetConfigSyntaxSetting = dotnetConfigSyntaxSetting.WithTrailingTrivia(comment);

                context.AddProperty(dotnetConfigSyntaxSetting);
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

    private IDotnetConfigPropertySyntaxNode ParseSetting(string key, EqualSymbolSyntaxNode equalSymbol, string value)
    {
        if (_generalRuleKeys.Contains(key))
            return new DotnetConfigGeneralOptionNode(key, equalSymbol, value);

        bool isSeveritySetting = key.StartsWith("dotnet_diagnostic.");
        if (isSeveritySetting)
        {
            string[] keyParts = key.Split('.');

            if (keyParts.Length != 3)
                throw new ArgumentException($"Incorrect rule key: {key}");

            if (!string.Equals(keyParts[2], "severity", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException($"Expect postfix .severity for diagnostic rule but was {keyParts[2]}");

            var ruleId = RoslynRuleId.Parse(keyParts[1]);
            return new DotnetConfigRuleSeverityNode(ruleId, equalSymbol, value);
        }

        bool isCompositeKeyRule = key.Contains('.');
        if (isCompositeKeyRule)
        {
            string[] keyParts = key.Split('.');
            return new DotnetConfigRuleCompositeOptionNode(keyParts, equalSymbol, value);
        }

        return new DotnetConfigRuleOptionNode(key, equalSymbol, value);
    }
}