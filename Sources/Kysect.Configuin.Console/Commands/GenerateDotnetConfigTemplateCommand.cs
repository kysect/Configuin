using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Template;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

public class GenerateDotnetConfigTemplateCommand(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    DotnetConfigDocumentTemplateGenerator dotnetConfigDocumentTemplateGenerator,
    ILogger logger
    ) : Command<GenerateDotnetConfigTemplateCommand.Settings>
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

        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        string editorconfigContent = dotnetConfigDocumentTemplateGenerator.GenerateTemplate(roslynRules);
        // TODO: move to interface?
        logger.LogInformation("Writing .editorconfig template to {path}", settings.DotnetConfigPath);
        File.WriteAllText(settings.DotnetConfigPath, editorconfigContent);
        return 0;
    }
}