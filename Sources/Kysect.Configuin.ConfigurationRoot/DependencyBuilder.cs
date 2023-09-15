using Kysect.CommonLib.DependencyInjection;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Markdown;
using Kysect.Configuin.ConfigurationRoot.Configuration;
using Kysect.Configuin.DotnetFormatIntegration;
using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.DotnetFormatIntegration.FileSystem;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.MsLearn;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kysect.Configuin.ConfigurationRoot;

public class DependencyBuilder
{
    public static IServiceCollection InitializeServiceProvider()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");

        IServiceCollection serviceCollection = builder.Services;

        serviceCollection.AddOptionsWithValidation<ConfiguinConfiguration>(nameof(ConfiguinConfiguration));
        serviceCollection.AddOptionsWithValidation<EditorConfigApplyConfiguration>(nameof(EditorConfigApplyConfiguration));
        serviceCollection.AddSingleton(CreateLogger);

        serviceCollection.AddSingleton<IEditorConfigContentProvider, EditorConfigFileContentProvider>();
        serviceCollection.AddSingleton<IEditorConfigSettingsParser, EditorConfigSettingsParser>();
        serviceCollection.AddSingleton<IMsLearnDocumentationInfoReader, MsLearnDocumentationInfoLocalReader>();

        serviceCollection.AddSingleton<IMsLearnDocumentationParser>(sp =>
        {
            ILogger logger = sp.GetRequiredService<ILogger>();
            return new MsLearnDocumentationParser(PlainTextExtractor.Create(), logger);
        });
        serviceCollection.AddSingleton<ICodeStyleGenerator, CodeStyleGenerator>();
        serviceCollection.AddSingleton<ICodeStyleWriter, MarkdownCodeStyleWriter>();

        serviceCollection.AddSingleton<DotnetFormatWarningGenerator>();
        serviceCollection.AddSingleton<TemporaryFileMover>();
        serviceCollection.AddSingleton<DotnetFormatReportComparator>();
        serviceCollection.AddSingleton<DotnetFormatPreviewGenerator>();
        serviceCollection.AddSingleton<EditorConfigTemplateGenerator>();

        return serviceCollection;
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