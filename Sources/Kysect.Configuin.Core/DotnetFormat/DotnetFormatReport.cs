namespace Kysect.Configuin.Core.DotnetFormat;

public record DotnetFormatFileChanges(int LineNumber, int CharNumber, string DiagnosticId, string FormatDescription);

public record DotnetFormatProjectId(Guid Id);

public record DotnetFormatDocumentId(Guid Id, DotnetFormatProjectId ProjectId);

public record DotnetFormatFileReport(
    DotnetFormatDocumentId DocumentId,
    string FileName,
    string FilePath,
    IReadOnlyCollection<DotnetFormatFileChanges> FileChanges);