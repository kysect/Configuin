using EditorConfigEditor.Core.CodeStyleGeneration;
using EditorConfigEditor.Core.EditorConfigModels;
using EditorConfigEditor.Core.EditorConfigParsing;
using EditorConfigEditor.Core.MicrosoftLearnDocumentation;
using EditorConfigEditor.Core.MicrosoftLearnDocumentation.Models;
using Microsoft.Extensions.DependencyInjection;

IServiceProvider sp = InitializeServiceProvider();
GenerateCodeStyle(sp);

IServiceProvider InitializeServiceProvider()
{
    var serviceCollection = new ServiceCollection();
    return serviceCollection.BuildServiceProvider();
}

void GenerateCodeStyle(IServiceProvider serviceProvider)
{
    IEditorConfigContentProvider editorConfigContentProvider = serviceProvider.GetRequiredService<IEditorConfigContentProvider>();
    IEditorConfigRuleParser editorConfigRuleParser = serviceProvider.GetRequiredService<IEditorConfigRuleParser>();
    IMicrosoftLearnDocumentationInfoProvider microsoftLearnDocumentationInfoProvider = serviceProvider.GetRequiredService<IMicrosoftLearnDocumentationInfoProvider>();
    IMicrosoftLearnDocumentationParser microsoftLearnDocumentationParser = serviceProvider.GetRequiredService<IMicrosoftLearnDocumentationParser>();
    ICodeStyleGenerator codeStyleGenerator = serviceProvider.GetRequiredService<ICodeStyleGenerator>();
    ICodeStyleWriter codeStyleWriter = serviceProvider.GetRequiredService<ICodeStyleWriter>();

    string editorConfigContent = editorConfigContentProvider.Provide();
    EditorConfigRuleSet editorConfigRuleSet = editorConfigRuleParser.Parse(editorConfigContent);

    MicrosoftLearnDocumentationRawInfo microsoftLearnDocumentationRawInfo = microsoftLearnDocumentationInfoProvider.Provide();
    RoslynRules roslynRules = microsoftLearnDocumentationParser.Parse(microsoftLearnDocumentationRawInfo);

    CodeStyleInfo codeStyleInfo = codeStyleGenerator.Generate(editorConfigRuleSet, roslynRules);
    codeStyleWriter.Write(codeStyleInfo);
}