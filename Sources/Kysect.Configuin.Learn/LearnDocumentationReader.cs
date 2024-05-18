using Kysect.Configuin.Learn.ContentParsing;

namespace Kysect.Configuin.Learn;

public class LearnDocumentationReader
{
    public LearnDocumentationFileContents Provide(string pathToRepository)
    {
        var msLearnRepositoryPathProvider = new LearnRepositoryPathProvider(pathToRepository);

        string qualityRulesDirectory = msLearnRepositoryPathProvider.GetPathToQualityRules();
        string styleRulesDirectory = msLearnRepositoryPathProvider.GetPathToStyleRules();
        string sharpFormattingOptions = msLearnRepositoryPathProvider.GetPathToSharpFormattingFile();
        string dotnetFormattingOptions = msLearnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string qualityRuleOptions = msLearnRepositoryPathProvider.GetPathToCodeQualityRuleOptions();

        IReadOnlyCollection<string> qualityRuleFileContents = Directory
            .EnumerateFiles(qualityRulesDirectory, "ca*.md")
            .Select(File.ReadAllText)
            .ToList();

        IReadOnlyCollection<string> styleRuleInfos = Directory
            .EnumerateFiles(styleRulesDirectory, "ide*.md")
            .Select(File.ReadAllText)
            .ToList();

        string sharpFormattingOptionsContent = File.ReadAllText(sharpFormattingOptions);
        string dotnetFormattingOptionsContent = File.ReadAllText(dotnetFormattingOptions);
        string qualityRuleOptionFileContent = File.ReadAllText(qualityRuleOptions);

        return new LearnDocumentationFileContents(
            qualityRuleFileContents,
            styleRuleInfos,
            sharpFormattingOptionsContent,
            dotnetFormattingOptionsContent,
            qualityRuleOptionFileContent);
    }
}