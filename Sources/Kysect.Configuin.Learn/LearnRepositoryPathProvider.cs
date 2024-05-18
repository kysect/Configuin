namespace Kysect.Configuin.Learn;

public class LearnRepositoryPathProvider(string root)
{
    public string GetPathToQualityRules()
    {
        return Path.Combine(root, "docs", "fundamentals", "code-analysis", "quality-rules");
    }

    public string GetPathToStyleRules()
    {
        return Path.Combine(root, "docs", "fundamentals", "code-analysis", "style-rules");
    }

    public string GetPathToSharpFormattingFile()
    {
        return Path.Combine(root, "docs", "fundamentals", "code-analysis", "style-rules", "csharp-formatting-options.md");
    }

    public string GetPathToDotnetFormattingFile()
    {
        return Path.Combine(root, "docs", "fundamentals", "code-analysis", "style-rules", "dotnet-formatting-options.md");
    }

    public string GetPathToCodeQualityRuleOptions()
    {
        return Path.Combine(root, "docs", "fundamentals", "code-analysis", "code-quality-rule-options.md");
    }
}