using Kysect.Configuin.Common;
using Kysect.Configuin.Console.Configuration;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Markdown;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kysect.Configuin.Console;

public class DependencyBuilder
{
    public static IServiceProvider InitializeServiceProvider()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");

        IServiceCollection serviceCollection = builder.Services;

        serviceCollection.AddOptionsWithValidation<ConfiguinConfiguration>(nameof(ConfiguinConfiguration));

        serviceCollection.AddSingleton<IEditorConfigContentProvider>(sp =>
        {
            IOptions<ConfiguinConfiguration> options = sp.GetRequiredService<IOptions<ConfiguinConfiguration>>();

            return new EditorConfigFileContentProvider(options.Value.EditorConfigFile);
        });

        serviceCollection.AddSingleton<IEditorConfigSettingsParser, EditorConfigSettingsParser>();

        serviceCollection.AddSingleton<IMsLearnDocumentationInfoProvider>(sp =>
        {
            IOptions<ConfiguinConfiguration> options = sp.GetRequiredService<IOptions<ConfiguinConfiguration>>();

            return new MsLearnDocumentationInfoLocalProvider(options.Value.MsLearnRepositoryPath);
        });

        serviceCollection.AddSingleton<IMsLearnDocumentationParser>(_ => new MsLearnDocumentationParser(PlainTextExtractor.Create()));
        serviceCollection.AddSingleton<ICodeStyleGenerator, CodeStyleGenerator>();
        serviceCollection.AddSingleton<ICodeStyleWriter>(sp =>
        {
            IOptions<ConfiguinConfiguration> options = sp.GetRequiredService<IOptions<ConfiguinConfiguration>>();

            return new MarkdownCodeStyleWriter(options.Value.OutputPath);
        });

        return serviceCollection.BuildServiceProvider();
    }
}