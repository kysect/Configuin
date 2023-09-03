using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Diff;
using System.Text.Json;

namespace Kysect.Configuin.Core.DotnetFormat;

public class DotnetFormatReportComparator
{
    public static CollectionDiff<DotnetFormatFileReport> Compare(string left, string right)
    {
        IReadOnlyCollection<DotnetFormatFileReport> leftReports = JsonSerializer.Deserialize<IReadOnlyCollection<DotnetFormatFileReport>>(left).ThrowIfNull();
        IReadOnlyCollection<DotnetFormatFileReport> rightReports = JsonSerializer.Deserialize<IReadOnlyCollection<DotnetFormatFileReport>>(right).ThrowIfNull();

        var diff = CollectionDiff.Create(leftReports, rightReports, DotnetFormatFileReportComparator.Instance);

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