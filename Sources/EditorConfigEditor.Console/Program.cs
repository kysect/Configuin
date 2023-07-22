using EditorConfigEditor.Core.CodeStyleGeneration;
using EditorConfigEditor.Core.EditorConfigParsing;
using EditorConfigEditor.Core.MsLearnDocumentation;
using EditorConfigEditor.Core.MsLearnDocumentation.Models;
using EditorConfigEditor.Core.RoslynRuleModels;
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
    IMsLearnDocumentationInfoProvider msLearnDocumentationInfoProvider = serviceProvider.GetRequiredService<IMsLearnDocumentationInfoProvider>();
    IMsLearnDocumentationParser msLearnDocumentationParser = serviceProvider.GetRequiredService<IMsLearnDocumentationParser>();
    ICodeStyleGenerator codeStyleGenerator = serviceProvider.GetRequiredService<ICodeStyleGenerator>();
    ICodeStyleWriter codeStyleWriter = serviceProvider.GetRequiredService<ICodeStyleWriter>();

    string editorConfigContent = editorConfigContentProvider.Provide();
    EditorConfigRuleSet editorConfigRuleSet = editorConfigRuleParser.Parse(editorConfigContent);

    MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = msLearnDocumentationInfoProvider.Provide();
    RoslynRules roslynRules = msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);

    CodeStyleInfo codeStyleInfo = codeStyleGenerator.Generate(editorConfigRuleSet, roslynRules);
    codeStyleWriter.Write(codeStyleInfo);
}