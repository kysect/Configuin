using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Formatter;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Kysect.Configuin.Console.Commands;

internal sealed class FormatDotnetConfigCommand(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    DotnetConfigDocumentParser dotnetConfigDocumentParser,
    DotnetConfigDocumentFormatter dotnetConfigDocumentFormatter
    ) : Command<FormatDotnetConfigCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to dotnet config file.")]
        [CommandArgument(0, "[dotnet config path]")]
        public string? DotnetConfigPath { get; init; }

        [Description("Path to cloned MS Learn repository.")]
        [CommandOption("-d|--documentation")]
        public string? MsLearnRepositoryPath { get; init; }

        [Description("Group CA rules by category")]
        [CommandOption("--group-ca")]
        [DefaultValue(true)]
        public bool GroupQualityRulesByCategory { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.DotnetConfigPath.ThrowIfNull();

        string[] editorConfigContentLines = File.ReadAllLines(settings.DotnetConfigPath);
        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        DotnetConfigDocument dotnetConfigDocument = dotnetConfigDocumentParser.Parse(editorConfigContentLines);
        DotnetConfigDocument formattedDocument = dotnetConfigDocumentFormatter.FormatAccordingToRuleDefinitions(dotnetConfigDocument, roslynRules, settings.GroupQualityRulesByCategory);
        File.WriteAllText(settings.DotnetConfigPath, formattedDocument.ToFullString());

        return 0;
    }
}