using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Analyzing;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class AnalyzeDotnetConfigCommand(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    DotnetConfigDocumentParser dotnetConfigDocumentParser,
    ILogger logger
    ) : Command<AnalyzeDotnetConfigCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to dotnet config file.")]
        [CommandArgument(0, "[dotnet config path]")]
        public string? DotnetConfigPath { get; init; }

        [Description("Path to cloned MS Learn repository.")]
        [CommandOption("-d|--documentation")]
        public string? MsLearnRepositoryPath { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.DotnetConfigPath.ThrowIfNull();

        DotnetConfigDocumentAnalyzer dotnetConfigDocumentAnalyzer = new DotnetConfigDocumentAnalyzer();
        IDotnetConfigAnalyzeReporter reporter = new DotnetConfigAnalyzeLogReporter(logger);

        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        string editorConfigContent = File.ReadAllText(settings.DotnetConfigPath);
        DotnetConfigDocument dotnetConfigDocument = dotnetConfigDocumentParser.Parse(editorConfigContent);

        DotnetConfigMissedConfiguration dotnetConfigMissedConfiguration = dotnetConfigDocumentAnalyzer.GetMissedConfigurations(dotnetConfigDocument, roslynRules);
        IReadOnlyCollection<DotnetConfigInvalidOptionValue> incorrectOptionValues = dotnetConfigDocumentAnalyzer.GetIncorrectOptionValues(dotnetConfigDocument, roslynRules);
        IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity = dotnetConfigDocumentAnalyzer.GetIncorrectOptionSeverity(dotnetConfigDocument, roslynRules);

        reporter.ReportMissedConfigurations(dotnetConfigMissedConfiguration);
        reporter.ReportIncorrectOptionValues(incorrectOptionValues);
        reporter.ReportIncorrectOptionSeverity(incorrectOptionSeverity);

        return 0;
    }
}