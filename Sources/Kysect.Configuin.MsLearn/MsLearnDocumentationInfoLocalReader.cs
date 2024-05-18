using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public class MsLearnDocumentationInfoLocalReader : IMsLearnDocumentationInfoReader
{
    public MsLearnDocumentationRawInfo Provide(string pathToRepository)
    {
        var msLearnRepositoryPathProvider = new MsLearnRepositoryPathProvider(pathToRepository);

        string qualityRulesDirectory = msLearnRepositoryPathProvider.GetPathToQualityRules();
        string styleRulesDirectory = msLearnRepositoryPathProvider.GetPathToStyleRules();
        string sharpFormattingOptions = msLearnRepositoryPathProvider.GetPathToSharpFormattingFile();
        string dotnetFormattingOptions = msLearnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string qualityRuleOptions = msLearnRepositoryPathProvider.GetPathToCodeQualityRuleOptions();

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
        string qualityRuleOptionsContent = File.ReadAllText(qualityRuleOptions);

        return new MsLearnDocumentationRawInfo(
            qualityRuleInfos,
            styleRuleInfos,
            sharpFormattingOptionsContent,
            dotnetFormattingOptionsContent,
            qualityRuleOptionsContent);
    }
}