using Kysect.Configuin.Core.MsLearnDocumentation.Models;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public class MsLearnDocumentationInfoLocalProvider : IMsLearnDocumentationInfoProvider
{
    private readonly string _pathToRepository;

    public MsLearnDocumentationInfoLocalProvider(string pathToRepository)
    {
        ArgumentException.ThrowIfNullOrEmpty(pathToRepository);

        _pathToRepository = pathToRepository;
    }

    public MsLearnDocumentationRawInfo Provide()
    {
        string qualityRulesDirectory = Path.Combine(_pathToRepository, @"docs\fundamentals\code-analysis\quality-rules\");
        string styleRulesDirectory = Path.Combine(_pathToRepository, @"docs\fundamentals\code-analysis\style-rules\");
        string sharpFormattingOptions = Path.Combine(_pathToRepository, @"docs\fundamentals\code-analysis\style-rules\csharp-formatting-options.md");
        string dotnetFormattingOptions = Path.Combine(_pathToRepository, @"docs\fundamentals\code-analysis\style-rules\dotnet-formatting-options.md");

        var qualityRuleInfos = Directory
            .EnumerateFiles(qualityRulesDirectory, "ca*.md")
            .Select(File.ReadAllText)
            .ToList();

        var styleRuleInfos = Directory
            .EnumerateFiles(styleRulesDirectory, "ide*.md")
            .Select(File.ReadAllText)
            .ToList();

        string sharpFormattingOptionsContent = File.ReadAllText(sharpFormattingOptions);
        string dotnetFormattingOptionsContent = File.ReadAllText(dotnetFormattingOptions);

        return new MsLearnDocumentationRawInfo(
            qualityRuleInfos,
            styleRuleInfos,
            sharpFormattingOptionsContent,
            dotnetFormattingOptionsContent);
    }
}