using Kysect.CommonLib.Collections.Diff;
using Kysect.CommonLib.Logging;
using Kysect.Configuin.Console.Configuration;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.FileSystem;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.DotnetFormatIntegration;
using Kysect.Configuin.RoslynModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    public void GetEditorConfigWarningUpdates()
    {
        EditorConfigApplyConfiguration editorConfigApplyConfiguration = _serviceProvider.GetRequiredService<IOptions<EditorConfigApplyConfiguration>>().Value;
        DotnetFormatWarningGenerator dotnetFormatWarningGenerator = _serviceProvider.GetRequiredService<DotnetFormatWarningGenerator>();
        TemporaryFileMover temporaryFileMover = _serviceProvider.GetRequiredService<TemporaryFileMover>();
        DotnetFormatReportComparator dotnetFormatReportComparator = _serviceProvider.GetRequiredService<DotnetFormatReportComparator>();
        ILogger logger = _serviceProvider.GetRequiredService<ILogger>();

        IReadOnlyCollection<DotnetFormatFileReport> originalWarnings = dotnetFormatWarningGenerator.GenerateWarnings(editorConfigApplyConfiguration.SolutionPath);
        IReadOnlyCollection<DotnetFormatFileReport> newWarnings;

        using (temporaryFileMover.MoveFile(editorConfigApplyConfiguration.NewEditorConfig, editorConfigApplyConfiguration.SourceEditorConfig))
            newWarnings = dotnetFormatWarningGenerator.GenerateWarnings(editorConfigApplyConfiguration.SolutionPath);

        CollectionDiff<DotnetFormatFileReport> warningDiff = dotnetFormatReportComparator.Compare(originalWarnings, newWarnings);

        logger.LogInformation($"New warnings count: {warningDiff.Added.Count}");
        foreach (DotnetFormatFileReport dotnetFormatFileReport in warningDiff.Added)
        {
            logger.LogTabInformation(1, $"{dotnetFormatFileReport.FilePath}");
            foreach (DotnetFormatFileChanges dotnetFormatFileChanges in dotnetFormatFileReport.FileChanges)
                logger.LogTabInformation(2, $"{dotnetFormatFileChanges.FormatDescription}");
        }
    }
}