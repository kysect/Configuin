﻿using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Commands;

internal sealed class GenerateCodeStyleDocCommand(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser,
    ICodeStyleGenerator codeStyleGenerator,
    ICodeStyleWriter codeStyleWriter,
    DotnetConfigDocumentParser documentParser
    ) : Command<GenerateCodeStyleDocCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to dotnet config file.")]
        [CommandArgument(0, "[dotnet-config-path]")]
        public string? DotnetConfigPath { get; init; }

        [Description("Path to generated document.")]
        [CommandOption("-o|--output")]
        public string? OutputPath { get; init; }

        [Description("Path to cloned MS Learn repository.")]
        [CommandOption("-d|--documentation")]
        public string? MsLearnRepositoryPath { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        settings.DotnetConfigPath.ThrowIfNull();
        settings.OutputPath.ThrowIfNull();

        string editorConfigContent = File.ReadAllText(settings.DotnetConfigPath);
        DotnetConfigDocument dotnetConfigDocument = documentParser.Parse(editorConfigContent);

        RoslynRules roslynRules = settings.MsLearnRepositoryPath is null
            ? RoslynRuleDocumentationCache.ReadFromCache()
            : roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);

        CodeStyle codeStyle = codeStyleGenerator.Generate(dotnetConfigDocument, roslynRules);
        codeStyleWriter.Write(settings.OutputPath, codeStyle);

        return 0;
    }
}