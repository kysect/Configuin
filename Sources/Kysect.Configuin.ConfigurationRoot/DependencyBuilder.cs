using Kysect.CommonLib.DependencyInjection;
using Kysect.Configuin.ConfigurationRoot.Configuration;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Markdown;
using Kysect.Configuin.DotnetFormatIntegration;
using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.DotnetFormatIntegration.FileSystem;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.MsLearn;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kysect.Configuin.ConfigurationRoot;

public class DependencyBuilder
{
    public static IServiceProvider InitializeServiceProvider()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");

        IServiceCollection serviceCollection = builder.Services;

        serviceCollection.AddOptionsWithValidation<ConfiguinConfiguration>(nameof(ConfiguinConfiguration));
        serviceCollection.AddOptionsWithValidation<EditorConfigApplyConfiguration>(nameof(EditorConfigApplyConfiguration));
        serviceCollection.AddSingleton(CreateLogger);

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

        serviceCollection.AddSingleton<IMsLearnDocumentationParser>(sp =>
        {
            ILogger logger = sp.GetRequiredService<ILogger>();
            return new MsLearnDocumentationParser(PlainTextExtractor.Create(), logger);
        });
        serviceCollection.AddSingleton<ICodeStyleGenerator, CodeStyleGenerator>();
        serviceCollection.AddSingleton<ICodeStyleWriter>(sp =>
        {
            IOptions<ConfiguinConfiguration> options = sp.GetRequiredService<IOptions<ConfiguinConfiguration>>();
            ILogger logger = sp.GetRequiredService<ILogger>();

            return new MarkdownCodeStyleWriter(options.Value.OutputPath, logger);
        });

        serviceCollection.AddSingleton<DotnetFormatWarningGenerator>();
        serviceCollection.AddSingleton<TemporaryFileMover>();
        serviceCollection.AddSingleton<DotnetFormatReportComparator>();
        serviceCollection.AddSingleton<DotnetFormatPreviewGenerator>();

        return serviceCollection.BuildServiceProvider();
    }

    public static ILogger CreateLogger(IServiceProvider serviceProvider)
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console();

        ILoggerFactory loggerFactory = new LoggerFactory()
            .DemystifyExceptions()
            .AddSerilog(loggerConfiguration.CreateLogger());

        return loggerFactory.CreateLogger("Tests");
    }
}