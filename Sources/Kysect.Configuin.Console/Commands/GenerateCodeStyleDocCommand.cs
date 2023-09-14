using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class GenerateCodeStyleDocCommand : Command<GenerateCodeStyleDocCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to editorconfig.")]
        [CommandArgument(0, "[editor config path]")]
        public string? EditorConfigPath { get; init; }

        [Description("Path to generated document.")]
        [CommandOption("-o|--output")]
        public string? OutputPath { get; init; }

        [Description("Path to cloned MS Learn repository.")]
        [CommandOption("-d|--documentation")]
        public string? MsLearnRepositoryPath { get; init; }
    }

    private readonly IEditorConfigContentProvider _editorConfigContentProvider;
    private readonly IEditorConfigSettingsParser _editorConfigSettingsParser;
    private readonly IMsLearnDocumentationInfoReader _msLearnDocumentationInfoReader;
    private readonly IMsLearnDocumentationParser _msLearnDocumentationParser;
    private readonly ICodeStyleGenerator _codeStyleGenerator;
    private readonly ICodeStyleWriter _codeStyleWriter;

    public GenerateCodeStyleDocCommand(
        IEditorConfigContentProvider editorConfigContentProvider,
        IEditorConfigSettingsParser editorConfigSettingsParser,
        IMsLearnDocumentationInfoReader msLearnDocumentationInfoReader,
        IMsLearnDocumentationParser msLearnDocumentationParser,
        ICodeStyleGenerator codeStyleGenerator,
        ICodeStyleWriter codeStyleWriter)
    {
        _editorConfigContentProvider = editorConfigContentProvider;
        _editorConfigSettingsParser = editorConfigSettingsParser;
        _msLearnDocumentationInfoReader = msLearnDocumentationInfoReader;
        _msLearnDocumentationParser = msLearnDocumentationParser;
        _codeStyleGenerator = codeStyleGenerator;
        _codeStyleWriter = codeStyleWriter;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();
        settings.OutputPath.ThrowIfNull();

        string editorConfigContent = _editorConfigContentProvider.Provide(settings.EditorConfigPath);
        EditorConfigSettings editorConfigSettings = _editorConfigSettingsParser.Parse(editorConfigContent);

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _msLearnDocumentationInfoReader.Provide(settings.MsLearnRepositoryPath);
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);

        CodeStyle codeStyle = _codeStyleGenerator.Generate(editorConfigSettings, roslynRules);
        _codeStyleWriter.Write(settings.OutputPath, codeStyle);

        return 0;
    }
}