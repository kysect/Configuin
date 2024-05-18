using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

public class GenerateEditorConfigTemplateTemplate(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    EditorConfigTemplateGenerator editorConfigTemplateGenerator,
    ILogger logger
    ) : Command<GenerateEditorConfigTemplateTemplate.Settings>
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

        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        string editorconfigContent = editorConfigTemplateGenerator.GenerateTemplate(roslynRules);
        // TODO: move to interface?
        logger.LogInformation("Writing .editorconfig template to {path}", settings.EditorConfigPath);
        File.WriteAllText(settings.EditorConfigPath, editorconfigContent);
        return 0;
    }
}