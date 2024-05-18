using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class GenerateCodeStyleDocCommand(
    IDotnetConfigSettingsParser dotnetConfigSettingsParser,
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    ICodeStyleGenerator codeStyleGenerator,
    ICodeStyleWriter codeStyleWriter,
    EditorConfigDocumentParser documentParser
    ) : Command<GenerateCodeStyleDocCommand.Settings>
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

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.EditorConfigPath.ThrowIfNull();
        settings.OutputPath.ThrowIfNull();

        string editorConfigContent = File.ReadAllText(settings.EditorConfigPath);
        EditorConfigDocument editorConfigDocument = documentParser.Parse(editorConfigContent);
        DotnetConfigSettings dotnetConfigSettings = dotnetConfigSettingsParser.Parse(editorConfigDocument);

        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        CodeStyle codeStyle = codeStyleGenerator.Generate(dotnetConfigSettings, roslynRules);
        codeStyleWriter.Write(settings.OutputPath, codeStyle);

        return 0;
    }
}