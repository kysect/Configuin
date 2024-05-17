using Kysect.Configuin.ConfigurationRoot;
using Kysect.Configuin.Console.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Kysect.Configuin.Console;

internal class Program
{
    public static void Main(string[] args)
    {

        IServiceCollection registrations = DependencyBuilder.InitializeServiceProvider();

        if (args.Length == 0)
            args = PrepareTestCommand();

        var registrar = new TypeRegistrar(registrations);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.AddCommand<GenerateCodeStyleDocCommand>("generate-styledoc");
            config.AddCommand<EditorConfigApplyPreviewCommand>("preview");
            config.AddCommand<AnalyzeEditorConfigCommand>("analyze");
            config.AddCommand<GenerateEditorConfigTemplateTemplate>("template");
            config.AddCommand<FormatEditorconfigCommand>("format");
        });

        app.Run(args);
    }

    private static string[] PrepareTestCommand()
    {
        var msLearnRepositoryPath = Path.Combine("..", "..", "..", "..", "..", "ms-learn");

        string[] analyzeCommand = new[] { "analyze", ".editorconfig", "-d", msLearnRepositoryPath };
        string[] templateGenerateCommand = new[] { "template", ".editorconfig", "-d", msLearnRepositoryPath };
        string[] formatCommand = new[] { "format", ".editorconfig", "-d", msLearnRepositoryPath };

        return formatCommand;
    }
}