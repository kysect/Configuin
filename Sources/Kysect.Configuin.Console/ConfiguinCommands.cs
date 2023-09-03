using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Microsoft.Extensions.DependencyInjection;

namespace Kysect.Configuin.Console;

public class ConfiguinCommands
{
    private readonly IServiceProvider _serviceProvider;

    public ConfiguinCommands(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void GenerateCodeStyle()
    {
        IEditorConfigContentProvider editorConfigContentProvider = _serviceProvider.GetRequiredService<IEditorConfigContentProvider>();
        IEditorConfigSettingsParser editorConfigSettingsParser = _serviceProvider.GetRequiredService<IEditorConfigSettingsParser>();
        IMsLearnDocumentationInfoProvider msLearnDocumentationInfoProvider = _serviceProvider.GetRequiredService<IMsLearnDocumentationInfoProvider>();
        IMsLearnDocumentationParser msLearnDocumentationParser = _serviceProvider.GetRequiredService<IMsLearnDocumentationParser>();
        ICodeStyleGenerator codeStyleGenerator = _serviceProvider.GetRequiredService<ICodeStyleGenerator>();
        ICodeStyleWriter codeStyleWriter = _serviceProvider.GetRequiredService<ICodeStyleWriter>();

        string editorConfigContent = editorConfigContentProvider.Provide();
        EditorConfigSettings editorConfigSettings = editorConfigSettingsParser.Parse(editorConfigContent);

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = msLearnDocumentationInfoProvider.Provide();
        RoslynRules roslynRules = msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);

        CodeStyle codeStyle = codeStyleGenerator.Generate(editorConfigSettings, roslynRules);
        codeStyleWriter.Write(codeStyle);
    }
}