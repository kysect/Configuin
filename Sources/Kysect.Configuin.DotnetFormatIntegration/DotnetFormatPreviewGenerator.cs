using Kysect.CommonLib.Collections.Diff;
using Kysect.CommonLib.Logging;
using Kysect.Configuin.DotnetFormatIntegration.Abstractions;
using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.DotnetFormatIntegration.FileSystem;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration;

public class DotnetFormatPreviewGenerator : IDotnetFormatPreviewGenerator
{
    private readonly DotnetFormatWarningGenerator _dotnetFormatWarningGenerator;
    private readonly TemporaryFileMover _temporaryFileMover;
    private readonly DotnetFormatReportComparator _dotnetFormatReportComparator;
    private readonly ILogger _logger;

    public DotnetFormatPreviewGenerator(
        DotnetFormatWarningGenerator dotnetFormatWarningGenerator,
        TemporaryFileMover temporaryFileMover,
        DotnetFormatReportComparator dotnetFormatReportComparator,
        ILogger logger)
    {
        _dotnetFormatWarningGenerator = dotnetFormatWarningGenerator;
        _temporaryFileMover = temporaryFileMover;
        _dotnetFormatReportComparator = dotnetFormatReportComparator;
        _logger = logger;
    }

    public void GetEditorConfigWarningUpdates(string solutionPath, string newEditorConfig, string sourceEditorConfig)
    {
        IReadOnlyCollection<DotnetFormatFileReport> originalWarnings = _dotnetFormatWarningGenerator.GenerateWarnings(solutionPath);

        IFileMoveUndoOperation undoOperation = _temporaryFileMover.MoveFile(newEditorConfig, sourceEditorConfig);
        IReadOnlyCollection<DotnetFormatFileReport> newWarnings = _dotnetFormatWarningGenerator.GenerateWarnings(solutionPath);
        undoOperation.Undo();

        CollectionDiff<DotnetFormatFileReport> warningDiff = _dotnetFormatReportComparator.Compare(originalWarnings, newWarnings);

        _logger.LogInformation("New warnings count: {Count}", warningDiff.Added.Count);
        foreach (DotnetFormatFileReport dotnetFormatFileReport in warningDiff.Added)
        {
            _logger.LogTabInformation(1, $"{dotnetFormatFileReport.FilePath}");
            foreach (DotnetFormatFileChanges dotnetFormatFileChanges in dotnetFormatFileReport.FileChanges)
                _logger.LogTabInformation(2, $"{dotnetFormatFileChanges.FormatDescription}");
        }
    }
}