using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Formatter;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Kysect.Configuin.Console.Commands;

internal sealed class FormatEditorconfigCommand(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    EditorConfigDocumentParser editorConfigDocumentParser,
    EditorConfigFormatter editorConfigFormatter
    ) : Command<FormatEditorconfigCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to editorconfig.")]
        [CommandArgument(0, "[editor config path]")]
        public string? EditorConfigPath { get; init; }

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
        settings.EditorConfigPath.ThrowIfNull();

        string[] editorConfigContentLines = File.ReadAllLines(settings.EditorConfigPath);
        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        EditorConfigDocument editorConfigDocument = editorConfigDocumentParser.Parse(editorConfigContentLines);
        EditorConfigDocument formattedDocument = editorConfigFormatter.FormatAccordingToRuleDefinitions(editorConfigDocument, roslynRules, settings.GroupQualityRulesByCategory);
        File.WriteAllText(settings.EditorConfigPath, formattedDocument.ToFullString());

        return 0;
    }
}