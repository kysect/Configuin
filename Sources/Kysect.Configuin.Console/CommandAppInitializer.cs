using Kysect.Configuin.Console.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Kysect.Configuin.Console;

public static class CommandAppInitializer
{
    public static CommandApp Initialize(IServiceCollection services)
    {
        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.AddCommand<GenerateCodeStyleDocCommand>("generate-styledoc");
            config.AddCommand<EditorConfigApplyPreviewCommand>("preview");
            config.AddCommand<AnalyzeEditorConfigCommand>("analyze");
            config.AddCommand<GenerateEditorConfigTemplateTemplate>("template");
            config.AddCommand<FormatEditorconfigCommand>("format");
            config.AddCommand<GenerateRoslynRuleDocumentationFile>("generate-roslyn-documentation");
        });

        return app;
    }
}