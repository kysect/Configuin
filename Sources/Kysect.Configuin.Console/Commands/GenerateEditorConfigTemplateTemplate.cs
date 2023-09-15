using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

public class GenerateEditorConfigTemplateTemplate : Command<GenerateEditorConfigTemplateTemplate.Settings>
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

    private readonly IMsLearnDocumentationInfoReader _msLearnDocumentationInfoReader;
    private readonly IMsLearnDocumentationParser _msLearnDocumentationParser;
    private readonly EditorConfigTemplateGenerator _editorConfigTemplateGenerator;
    private readonly ILogger _logger;

    public GenerateEditorConfigTemplateTemplate(
        IMsLearnDocumentationInfoReader msLearnDocumentationInfoReader,
        IMsLearnDocumentationParser msLearnDocumentationParser,
        EditorConfigTemplateGenerator editorConfigTemplateGenerator,
        ILogger logger)
    {
        _msLearnDocumentationInfoReader = msLearnDocumentationInfoReader;
        _msLearnDocumentationParser = msLearnDocumentationParser;
        _editorConfigTemplateGenerator = editorConfigTemplateGenerator;
        _logger = logger;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _msLearnDocumentationInfoReader.Provide(settings.MsLearnRepositoryPath);
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        string editorconfigContent = _editorConfigTemplateGenerator.GenerateTemplate(roslynRules);
        // TODO: move to interface?
        _logger.LogInformation("Writing .editorconfig template to {path}", settings.EditorConfigPath);
        File.WriteAllText(settings.EditorConfigPath, editorconfigContent);
        return 0;
    }
}