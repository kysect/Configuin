using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Formatter;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Kysect.Configuin.Console.Commands;

internal sealed class FormatEditorconfigCommand(
    IMsLearnDocumentationInfoReader repositoryPathReader,
    IMsLearnDocumentationParser msLearnDocumentationParser,
    EditorConfigDocumentParser editorConfigDocumentParser,
    EditorConfigFormatter editorConfigFormatter) : Command<FormatEditorconfigCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to editorconfig.")]
        [CommandArgument(0, "[editor config path]")]
        public string? EditorConfigPath { get; init; }

        [Description("Path to cloned MS Learn repository.")]
        [CommandOption("-d|--documentation")]
        public string? MsLearnRepositoryPath { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();

        string editorConfigContent = File.ReadAllText(settings.EditorConfigPath);
        string[] editorConfigContentLines = File.ReadAllLines(settings.EditorConfigPath);

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = repositoryPathReader.Provide(settings.MsLearnRepositoryPath);
        RoslynRules roslynRules = msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        EditorConfigDocument editorConfigDocument = editorConfigDocumentParser.Parse(editorConfigContentLines);
        EditorConfigDocument formattedDocument = editorConfigFormatter.FormatAccordingToRuleDefinitions(editorConfigDocument, roslynRules);
        File.WriteAllText(settings.EditorConfigPath, formattedDocument.ToFullString());

        return 0;
    }
}