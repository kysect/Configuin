namespace Kysect.Configuin.MsLearn;

public class MsLearnRepositoryPathProvider
{
    private readonly string _root;

    public MsLearnRepositoryPathProvider(string root)
    {
        _root = root;
    }

    public string GetPathToQualityRules()
    {
        return Path.Combine(_root, "docs", "fundamentals", "code-analysis", "quality-rules");
    }

    public string GetPathToStyleRules()
    {
        return Path.Combine(_root, "docs", "fundamentals", "code-analysis", "style-rules");
    }

    public string GetPathToSharpFormattingFile()
    {
        return Path.Combine(_root, "docs", "fundamentals", "code-analysis", "style-rules", "csharp-formatting-options.md");
    }

    public string GetPathToDotnetFormattingFile()
    {
        return Path.Combine(_root, "docs", "fundamentals", "code-analysis", "style-rules", "dotnet-formatting-options.md");
    }

    public string GetPathToCodeQualityRuleOptions()
    {
        return Path.Combine(_root, "docs", "fundamentals", "code-analysis", "code-quality-rule-options.md");
    }
}