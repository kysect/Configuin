using Kysect.Configuin.Core.DotnetFormat;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.DotnetFormat;

public class DotnetFormatCliTests
{
    private readonly DotnetFormatCli _dotnetFormatCli;

    public DotnetFormatCliTests()
    {
        _dotnetFormatCli = new DotnetFormatCli(TestLogger.ProviderForTests());
    }

    [Test]
    [Ignore("This test require infrastructure")]
    public void Validate_FinishedWithoutErrors()
    {
        _dotnetFormatCli.Validate();
    }

    [Test]
    //[Ignore("This test require infrastructure")]
    public void GenerateWarnings_CreateReportFile()
    {
        //const string pathToSln = "./../../../../";
        const string pathToSln = "C:\\Coding\\Kysect.PowerShellRunner\\Sources\\Kysect.PowerShellRunner.sln";

        _dotnetFormatCli.Format(pathToSln, "sample.json");
    }
}