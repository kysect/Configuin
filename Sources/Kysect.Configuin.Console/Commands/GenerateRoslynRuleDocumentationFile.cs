using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.RoslynModels;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

namespace Kysect.Configuin.Console.Commands;

public class GenerateRoslynRuleDocumentationFile(
    IRoslynRuleDocumentationParser roslynRuleDocumentationParser
) : Command<GenerateRoslynRuleDocumentationFile.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to cloned MS Learn repository.")]
        [CommandArgument(0, "[ms-repo-path]")]
        public string? MsLearnRepositoryPath { get; init; }

        [Description("Output path.")]
        [CommandArgument(0, "[output-path]")]
        public string? OutputPath { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.ThrowIfNull();
        settings.MsLearnRepositoryPath.ThrowIfNull();
        settings.OutputPath.ThrowIfNull();

        RoslynRules roslynRules = roslynRuleDocumentationParser.Parse(settings.MsLearnRepositoryPath);
        string documentation = JsonSerializer.Serialize(roslynRules);
        File.WriteAllText(settings.OutputPath, documentation);
        return 0;
    }
}