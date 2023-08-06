using FluentAssertions;
using Kysect.Configuin.Core.MsLearnDocumentation;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class MsLearnRepositoryPathProviderTests
{
    [Test]
    public void GetPath_ReturnExistsFileItems()
    {
        string pathToRoot = Path.Combine(
            "..", // net7.0
            "..", // Debug
            "..", // bin
            "..", // Kysect.Configuin.Tests
            "..", // root
            "ms-learn");

        var pathProvider = new MsLearnRepositoryPathProvider(pathToRoot);

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