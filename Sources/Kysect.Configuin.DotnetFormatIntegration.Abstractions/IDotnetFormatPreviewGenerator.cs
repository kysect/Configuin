namespace Kysect.Configuin.DotnetFormatIntegration.Abstractions;

public interface IDotnetFormatPreviewGenerator
{
    void GetEditorConfigWarningUpdates(string solutionPath, string newEditorConfig, string sourceEditorConfig);
}