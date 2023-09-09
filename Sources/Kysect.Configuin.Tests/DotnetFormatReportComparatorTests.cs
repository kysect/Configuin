using FluentAssertions;
using Kysect.CommonLib.Collections.Diff;
using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class DotnetFormatReportComparatorTests
{
    private readonly DotnetFormatReportComparator _dotnetFormatReportComparator;

    public DotnetFormatReportComparatorTests()
    {
        _dotnetFormatReportComparator = new DotnetFormatReportComparator(TestLogger.ProviderForTests());
    }

    [Test]
    public void Compare_ChangedFile_ReturnExpectedDiff()
    {
        string left = File.ReadAllText(Path.Combine("Resources", "DotnetFormatOutput-one-report.json"));
        string right = File.ReadAllText(Path.Combine("Resources", "DotnetFormatOutput-two-report.json"));

        CollectionDiff<DotnetFormatFileReport> diff = _dotnetFormatReportComparator.Compare(left, right);

        diff.Added.Should().HaveCount(1);
        diff.Same.Should().HaveCount(1);
    }
}