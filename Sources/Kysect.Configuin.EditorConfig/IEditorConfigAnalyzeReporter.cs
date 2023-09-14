namespace Kysect.Configuin.EditorConfig;

public interface IEditorConfigAnalyzeReporter
{
    void ReportMissedConfigurations(EditorConfigMissedConfiguration editorConfigMissedConfiguration);
    void ReportIncorrectOptionValues(IReadOnlyCollection<EditorConfigInvalidOptionValue> incorrectOptionValues);
}