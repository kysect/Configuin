using Kysect.Configuin.Common;
using Kysect.Configuin.Console.Configuration;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IServiceProvider serviceProvider = InitializeServiceProvider();
GenerateCodeStyle(serviceProvider);

IServiceProvider InitializeServiceProvider()
{
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddOptionsWithValidation<ConfiguinConfiguration>(nameof(ConfiguinConfiguration));

    serviceCollection.AddSingleton<IMsLearnDocumentationInfoProvider>(sp =>
    {
        IOptions<ConfiguinConfiguration> options = sp.GetRequiredService<IOptions<ConfiguinConfiguration>>();

        return new MsLearnDocumentationInfoLocalProvider(options.Value.MsLearnRepositoryPath);
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