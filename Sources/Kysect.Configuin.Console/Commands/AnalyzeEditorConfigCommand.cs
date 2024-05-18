using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Analyzing;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class AnalyzeEditorConfigCommand(
    IDotnetConfigSettingsParser dotnetConfigSettingsParser,
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    EditorConfigDocumentParser editorConfigDocumentParser,
    ILogger logger
    ) : Command<AnalyzeEditorConfigCommand.Settings>
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

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();

        EditorConfigAnalyzer editorConfigAnalyzer = new EditorConfigAnalyzer();
        IEditorConfigAnalyzeReporter reporter = new EditorConfigAnalyzeLogReporter(logger);

        string editorConfigContent = File.ReadAllText(settings.EditorConfigPath);
        EditorConfigDocument editorConfigDocument = editorConfigDocumentParser.Parse(editorConfigContent);
        DotnetConfigSettings dotnetConfigSettings = dotnetConfigSettingsParser.Parse(editorConfigDocument);
        RoslynRules roslynRules = roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        EditorConfigMissedConfiguration editorConfigMissedConfiguration = editorConfigAnalyzer.GetMissedConfigurations(dotnetConfigSettings, roslynRules);
        IReadOnlyCollection<EditorConfigInvalidOptionValue> incorrectOptionValues = editorConfigAnalyzer.GetIncorrectOptionValues(dotnetConfigSettings, roslynRules);
        IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity = editorConfigAnalyzer.GetIncorrectOptionSeverity(dotnetConfigSettings, roslynRules);

        reporter.ReportMissedConfigurations(editorConfigMissedConfiguration);
        reporter.ReportIncorrectOptionValues(incorrectOptionValues);
        reporter.ReportIncorrectOptionSeverity(incorrectOptionSeverity);

        return 0;
    }
}