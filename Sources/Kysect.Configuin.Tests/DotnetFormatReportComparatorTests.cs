using FluentAssertions;
using Kysect.CommonLib.Collections.Diff;
using Kysect.Configuin.Core.DotnetFormat;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class DotnetFormatReportComparatorTests
{
    [Test]
    public void Compare_ChangedFile_ReturnExpectedDiff()
    {
        string left = File.ReadAllText(Path.Combine("Resources", "DotnetFormatOutput-one-report.json"));
        string right = File.ReadAllText(Path.Combine("Resources", "DotnetFormatOutput-two-report.json"));

        CollectionDiff<DotnetFormatFileReport> diff = DotnetFormatReportComparator.Compare(left, right);

        diff.Added.Should().HaveCount(1);
        diff.Same.Should().HaveCount(1);
    }
}