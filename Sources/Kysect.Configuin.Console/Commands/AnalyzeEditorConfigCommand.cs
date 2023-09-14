using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Logging;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class AnalyzeEditorConfigCommand : Command<AnalyzeEditorConfigCommand.Settings>
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

    private readonly IEditorConfigContentProvider _editorConfigContentProvider;
    private readonly IEditorConfigSettingsParser _editorConfigSettingsParser;
    private readonly IMsLearnDocumentationInfoReader _msLearnDocumentationInfoReader;
    private readonly IMsLearnDocumentationParser _msLearnDocumentationParser;
    private readonly ILogger _logger;

    public AnalyzeEditorConfigCommand(IEditorConfigContentProvider editorConfigContentProvider, IEditorConfigSettingsParser editorConfigSettingsParser, IMsLearnDocumentationInfoReader msLearnDocumentationInfoReader, IMsLearnDocumentationParser msLearnDocumentationParser, ILogger logger)
    {
        _editorConfigContentProvider = editorConfigContentProvider;
        _editorConfigSettingsParser = editorConfigSettingsParser;
        _msLearnDocumentationInfoReader = msLearnDocumentationInfoReader;
        _msLearnDocumentationParser = msLearnDocumentationParser;
        _logger = logger;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();

        var editorConfigAnalyzer = new EditorConfigAnalyzer();

        string editorConfigContent = _editorConfigContentProvider.Provide(settings.EditorConfigPath);
        EditorConfigSettings editorConfigSettings = _editorConfigSettingsParser.Parse(editorConfigContent);
        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _msLearnDocumentationInfoReader.Provide(settings.MsLearnRepositoryPath);
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        EditorConfigMissedConfiguration editorConfigMissedConfiguration = editorConfigAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        if (editorConfigMissedConfiguration.StyleRuleSeverity.Any())
        {
            _logger.LogInformation("Missed style rules:");
            foreach (RoslynRuleId roslynRuleId in editorConfigMissedConfiguration.StyleRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (editorConfigMissedConfiguration.QualityRuleSeverity.Any())
        {
            _logger.LogInformation("Missed quality rules:");
            foreach (RoslynRuleId roslynRuleId in editorConfigMissedConfiguration.QualityRuleSeverity)
                _logger.LogTabInformation(1, roslynRuleId.ToString());
        }

        if (editorConfigMissedConfiguration.StyleRuleOptions.Any())
        {
            _logger.LogInformation("Missed options:");
            foreach (string styleRuleOption in editorConfigMissedConfiguration.StyleRuleOptions)
                _logger.LogTabInformation(1, styleRuleOption);
        }

        return 0;
    }
}