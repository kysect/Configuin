using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Diff;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kysect.Configuin.DotnetFormatIntegration.Cli;

public class DotnetFormatReportComparator
{
    private readonly ILogger _logger;

    public DotnetFormatReportComparator(ILogger logger)
    {
        _logger = logger;
    }

    public CollectionDiff<DotnetFormatFileReport> Compare(string left, string right)
    {
        _logger.LogInformation("Loading warnings for {left} and {right}", left, right);

        IReadOnlyCollection<DotnetFormatFileReport> leftReports = JsonSerializer.Deserialize<IReadOnlyCollection<DotnetFormatFileReport>>(left).ThrowIfNull();
        IReadOnlyCollection<DotnetFormatFileReport> rightReports = JsonSerializer.Deserialize<IReadOnlyCollection<DotnetFormatFileReport>>(right).ThrowIfNull();

        return Compare(leftReports, rightReports);
    }

    public CollectionDiff<DotnetFormatFileReport> Compare(IReadOnlyCollection<DotnetFormatFileReport> left, IReadOnlyCollection<DotnetFormatFileReport> right)
    {
        _logger.LogInformation("Comparing dotnet format report");
        var diff = CollectionDiff.Create(left, right, DotnetFormatFileReportComparator.Instance);
        _logger.LogInformation("Same: {same}, added: {added}, removed: {removed}", diff.Same.Count, diff.Added.Count, diff.Removed.Count);
        return diff;
    }

    private class DotnetFormatFileReportComparator : IEqualityComparer<DotnetFormatFileReport>
    {
        public static DotnetFormatFileReportComparator Instance { get; } = new DotnetFormatFileReportComparator();

        public bool Equals(DotnetFormatFileReport? x, DotnetFormatFileReport? y)
        {

            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return
                x.DocumentId.Equals(y.DocumentId)
                && x.FileName == y.FileName
                && x.FilePath == y.FilePath
                && x.FileChanges.SequenceEqual(y.FileChanges);
        }

        public int GetHashCode(DotnetFormatFileReport obj)
        {
            int hashCode = HashCode.Combine(obj.DocumentId, obj.FileName, obj.FilePath);

            foreach (DotnetFormatFileChanges changes in obj.FileChanges)
                hashCode = HashCode.Combine(hashCode, changes);

            return hashCode;
        }
    }
}