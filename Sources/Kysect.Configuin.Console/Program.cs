using Kysect.Configuin.ConfigurationRoot;
using Kysect.Configuin.ConfigurationRoot.Configuration;
using Kysect.Configuin.Console.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Kysect.Configuin.Console;

internal class Program
{
    public static void Main(string[] args)
    {

        IServiceCollection registrations = DependencyBuilder.InitializeServiceProvider();

#if DEBUG
        if (args.Length == 0)
            args = PrepareTestCommand(registrations);
#endif

        var registrar = new TypeRegistrar(registrations);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            // TODO: rename to styledoc
            config.AddCommand<GenerateCodeStyleDocCommand>("generate-codestyle-doc");
            config.AddCommand<EditorConfigApplyPreviewCommand>("preview");
            config.AddCommand<AnalyzeEditorConfigCommand>("analyze");
            config.AddCommand<GenerateEditorConfigTemplateTemplate>("template");
        });

        app.Run(args);
    }

    private static string[] PrepareTestCommand(IServiceCollection registrations)
    {
        ServiceProvider serviceProvider = registrations.BuildServiceProvider();
        ConfiguinConfiguration configurationOption = serviceProvider.GetRequiredService<IOptions<ConfiguinConfiguration>>().Value;

        string[] analyzeCommand = new[] { "analyze", configurationOption.EditorConfigFile, "-d", configurationOption.MsLearnRepositoryPath };
        string[] templateGenerateCommand = new[] { "template", ".editorconfig", "-d", configurationOption.MsLearnRepositoryPath };

        return templateGenerateCommand;
    }
}