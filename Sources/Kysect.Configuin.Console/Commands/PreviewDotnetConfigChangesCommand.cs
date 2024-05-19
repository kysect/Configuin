using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetFormatIntegration.Abstractions;
using Spectre.Console.Cli;

namespace Kysect.Configuin.Console.Commands;

internal sealed class PreviewDotnetConfigChangesCommand(
    IDotnetFormatPreviewGenerator dotnetFormatPreviewGenerator
    ) : Command<PreviewDotnetConfigChangesCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-s|--solution")]
        public string SolutionPath { get; init; } = null!;
        // TODO: remove this argument, take solution directory path
        [CommandOption("--current")]
        public string CurrentDotnetConfig { get; init; } = null!;
        [CommandOption("--new")]
        public string NewDotnetConfig { get; init; } = null!;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.SolutionPath.ThrowIfNull();
        settings.CurrentDotnetConfig.ThrowIfNull();
        settings.NewDotnetConfig.ThrowIfNull();


        dotnetFormatPreviewGenerator.GetWarningsAfterChangingDotnetConfig(
            settings.SolutionPath,
            settings.NewDotnetConfig,
            settings.CurrentDotnetConfig);

        return 0;
    }
}