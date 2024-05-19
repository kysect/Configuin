using Kysect.Configuin.ConfigurationRoot;
using Kysect.Configuin.Console;
using Microsoft.Extensions.DependencyInjection;

namespace Kysect.Configuin;

internal class Program
{
    private static void Main(string[] args)
    {
        IServiceCollection registrations = DependencyBuilder.InitializeServiceProvider();

        if (args.Length == 0)
            args = PrepareTestCommand();

        var app = CommandAppInitializer.Initialize(registrations);
        app.Run(args);
    }

    private static string[] PrepareTestCommand()
    {
        var msLearnRepositoryPath = Path.Combine("..", "..", "..", "..", "..", "ms-learn");

        string[] analyzeCommand = new[] { "analyze", ".editorconfig", "-d", msLearnRepositoryPath };
        string[] templateGenerateCommand = new[] { "template", ".editorconfig", "-d", msLearnRepositoryPath };
        string[] formatCommand = new[] { "format", ".editorconfig", "-d", msLearnRepositoryPath };
        string[] generateRoslynDocumentationCommand = new[] { "generate-roslyn-documentation", msLearnRepositoryPath, "roslyn-rules.json" };

        return generateRoslynDocumentationCommand;
    }
}