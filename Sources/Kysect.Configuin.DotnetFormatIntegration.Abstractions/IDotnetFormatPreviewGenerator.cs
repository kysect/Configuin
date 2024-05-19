namespace Kysect.Configuin.DotnetFormatIntegration.Abstractions;

public interface IDotnetFormatPreviewGenerator
{
    void GetWarningsAfterChangingDotnetConfig(string solutionPath, string newDotnetConfig, string sourceDotnetConfig);
}