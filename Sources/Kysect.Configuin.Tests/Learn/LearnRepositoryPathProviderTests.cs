using Kysect.Configuin.Learn;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.Learn;

public class LearnRepositoryPathProviderTests
{
    [Fact]
    public void GetPath_ReturnExistsFileItems()
    {
        string pathToRoot = Constants.GetPathToMsDocsRoot();

        var pathProvider = new LearnRepositoryPathProvider(pathToRoot);

        string pathToStyleRules = pathProvider.GetPathToStyleRules();
        string pathToQualityRules = pathProvider.GetPathToQualityRules();
        string pathToSharpFormattingFile = pathProvider.GetPathToSharpFormattingFile();
        string pathToDotnetFormattingFile = pathProvider.GetPathToDotnetFormattingFile();

        var directoryInfo = new DirectoryInfo(pathToRoot);
        Directory.Exists(directoryInfo.FullName).Should().BeTrue($"Directory {directoryInfo.FullName} must exist");

        Directory.Exists(pathToStyleRules).Should().BeTrue($"Directory {pathToStyleRules} must exist");
        Directory.Exists(pathToQualityRules).Should().BeTrue($"Directory {pathToQualityRules} must exist");

        File.Exists(pathToSharpFormattingFile).Should().BeTrue($"File {pathToSharpFormattingFile} must exist");
        File.Exists(pathToDotnetFormattingFile).Should().BeTrue($"File {pathToDotnetFormattingFile} must exist");
    }
}