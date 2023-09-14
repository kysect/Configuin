using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Analyzing;

public interface IEditorConfigAnalyzeReporter
{
    void ReportMissedConfigurations(EditorConfigMissedConfiguration editorConfigMissedConfiguration);
    void ReportIncorrectOptionValues(IReadOnlyCollection<EditorConfigInvalidOptionValue> incorrectOptionValues);
    void ReportIncorrectOptionSeverity(IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity);
}