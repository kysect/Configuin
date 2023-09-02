using Kysect.Configuin.Common;
using Kysect.Configuin.Console.Configuration;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Markdown;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

GenerateCodeStyle(InitializeServiceProvider());

IServiceProvider InitializeServiceProvider()
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
    builder.Configuration.AddJsonFile("appsettings.json");

    var serviceCollection = builder.Services;

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

void GenerateCodeStyle(IServiceProvider serviceProvider)
{
    IEditorConfigContentProvider editorConfigContentProvider = serviceProvider.GetRequiredService<IEditorConfigContentProvider>();
    IEditorConfigSettingsParser editorConfigSettingsParser = serviceProvider.GetRequiredService<IEditorConfigSettingsParser>();
    IMsLearnDocumentationInfoProvider msLearnDocumentationInfoProvider = serviceProvider.GetRequiredService<IMsLearnDocumentationInfoProvider>();
    IMsLearnDocumentationParser msLearnDocumentationParser = serviceProvider.GetRequiredService<IMsLearnDocumentationParser>();
    ICodeStyleGenerator codeStyleGenerator = serviceProvider.GetRequiredService<ICodeStyleGenerator>();
    ICodeStyleWriter codeStyleWriter = serviceProvider.GetRequiredService<ICodeStyleWriter>();

    string editorConfigContent = editorConfigContentProvider.Provide();
    EditorConfigSettings editorConfigSettings = editorConfigSettingsParser.Parse(editorConfigContent);

    MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = msLearnDocumentationInfoProvider.Provide();
    RoslynRules roslynRules = msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);

    CodeStyle codeStyle = codeStyleGenerator.Generate(editorConfigSettings, roslynRules);
    codeStyleWriter.Write(codeStyle);
}