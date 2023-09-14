using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetFormatIntegration;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class EditorConfigApplyPreviewCommand : Command<EditorConfigApplyPreviewCommand.Settings>
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

    private readonly DotnetFormatPreviewGenerator _dotnetFormatPreviewGenerator;

    public EditorConfigApplyPreviewCommand(DotnetFormatPreviewGenerator dotnetFormatPreviewGenerator)
    {
        _dotnetFormatPreviewGenerator = dotnetFormatPreviewGenerator;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.SolutionPath.ThrowIfNull();
        settings.SourceEditorConfig.ThrowIfNull();
        settings.NewEditorConfig.ThrowIfNull();


        _dotnetFormatPreviewGenerator.GetEditorConfigWarningUpdates(
            settings.SolutionPath,
            settings.NewEditorConfig,
            settings.SourceEditorConfig);

        return 0;
    }
}