using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Markdown;
using Kysect.Configuin.DotnetFormatIntegration;
using Kysect.Configuin.DotnetFormatIntegration.Abstractions;
using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.DotnetFormatIntegration.FileSystem;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.Formatter;
using Kysect.Configuin.EditorConfig.Template;
using Kysect.Configuin.Learn;
using Kysect.Configuin.Learn.Abstraction;
using Kysect.Configuin.Markdown.TextExtractor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kysect.Configuin.ConfigurationRoot;

// TODO: add tests for configuration
public static class DependencyBuilder
{
    public static IServiceCollection InitializeServiceProvider()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");

        IServiceCollection serviceCollection = builder.Services;

        serviceCollection.AddSingleton(CreateLogger);

        serviceCollection.AddSingleton<IDotnetFormatPreviewGenerator, DotnetFormatPreviewGenerator>();

        serviceCollection.AddSingleton<IRoslynRuleDocumentationParser, LearnDocumentationParser>();
        serviceCollection.AddSingleton<IMarkdownTextExtractor>(PlainTextExtractor.Create());
        serviceCollection.AddSingleton<ICodeStyleGenerator, CodeStyleGenerator>();
        serviceCollection.AddSingleton<ICodeStyleWriter, MarkdownCodeStyleWriter>();

        serviceCollection.AddSingleton<DotnetFormatWarningGenerator>();
        serviceCollection.AddSingleton<TemporaryFileMover>();
        serviceCollection.AddSingleton<DotnetFormatReportComparator>();
        serviceCollection.AddSingleton<DotnetFormatPreviewGenerator>();
        serviceCollection.AddSingleton<EditorConfigTemplateGenerator>();
        serviceCollection.AddSingleton<EditorConfigDocumentParser>();
        serviceCollection.AddSingleton<EditorConfigFormatter>();

        return serviceCollection;
    }

    public static ILogger CreateLogger(IServiceProvider serviceProvider)
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console();

        using var factory = new LoggerFactory();

        return factory
            .DemystifyExceptions()
            .AddSerilog(loggerConfiguration.CreateLogger())
            .CreateLogger("Tests");
    }
}