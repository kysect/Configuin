using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public class MsLearnDocumentationInfoLocalProvider : IMsLearnDocumentationInfoProvider
{
    private readonly MsLearnRepositoryPathProvider _repositoryPathProvider;

    public MsLearnDocumentationInfoLocalProvider(string pathToRepository)
    {
        ArgumentException.ThrowIfNullOrEmpty(pathToRepository);

        _repositoryPathProvider = new MsLearnRepositoryPathProvider(pathToRepository);
    }

    public MsLearnDocumentationRawInfo Provide()
    {
        string qualityRulesDirectory = _repositoryPathProvider.GetPathToQualityRules();
        string styleRulesDirectory = _repositoryPathProvider.GetPathToStyleRules();
        string sharpFormattingOptions = _repositoryPathProvider.GetPathToSharpFormattingFile();
        string dotnetFormattingOptions = _repositoryPathProvider.GetPathToDotnetFormattingFile();

        IReadOnlyCollection<string> qualityRuleInfos = Directory
            .EnumerateFiles(qualityRulesDirectory, "ca*.md")
            .Select(File.ReadAllText)
            .ToList();

        IReadOnlyCollection<string> styleRuleInfos = Directory
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