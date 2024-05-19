using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetFormatIntegration.Abstractions;
using Spectre.Console.Cli;

namespace Kysect.Configuin.Console.Commands;

internal sealed class EditorConfigApplyPreviewCommand(
    IDotnetFormatPreviewGenerator dotnetFormatPreviewGenerator
    ) : Command<EditorConfigApplyPreviewCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-s|--solution")]
        public string SolutionPath { get; init; } = null!;
        // TODO: remove this argument, take solution directory path
        [CommandOption("-t|--target")]
        public string SourceEditorConfig { get; init; } = null!;
        [CommandOption("-e|--editorconfig")]
        public string NewEditorConfig { get; init; } = null!;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.SolutionPath.ThrowIfNull();
        settings.SourceEditorConfig.ThrowIfNull();
        settings.NewEditorConfig.ThrowIfNull();


        dotnetFormatPreviewGenerator.GetEditorConfigWarningUpdates(
            settings.SolutionPath,
            settings.NewEditorConfig,
            settings.SourceEditorConfig);

        return 0;
    }
}