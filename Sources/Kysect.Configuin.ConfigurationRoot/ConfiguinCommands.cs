using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.ConfigurationRoot.Configuration;
using Kysect.Configuin.DotnetFormatIntegration;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kysect.Configuin.ConfigurationRoot;

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

    public void GetEditorConfigWarningUpdates()
    {
        EditorConfigApplyConfiguration editorConfigApplyConfiguration = _serviceProvider.GetRequiredService<IOptions<EditorConfigApplyConfiguration>>().Value;

        DotnetFormatPreviewGenerator dotnetFormatPreviewGenerator = _serviceProvider.GetRequiredService<DotnetFormatPreviewGenerator>();
        dotnetFormatPreviewGenerator.GetEditorConfigWarningUpdates(
            editorConfigApplyConfiguration.SolutionPath,
            editorConfigApplyConfiguration.NewEditorConfig,
            editorConfigApplyConfiguration.SourceEditorConfig);
    }
}