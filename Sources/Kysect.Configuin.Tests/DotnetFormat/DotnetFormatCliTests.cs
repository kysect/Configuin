using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.DotnetFormat;

public class DotnetFormatCliTests
{
    private readonly DotnetFormatCli _dotnetFormatCli;

    public DotnetFormatCliTests()
    {
        _dotnetFormatCli = new DotnetFormatCli(TestLogger.ProviderForTests());
    }

    [Fact(Skip = "This test require infrastructure")]
    public void Validate_FinishedWithoutErrors()
    {
        _dotnetFormatCli.Validate();
    }

    [Fact(Skip = "This test require infrastructure")]
    public void GenerateWarnings_CreateReportFile()
    {
        //const string pathToSln = "./../../../../";
        const string pathToSln = "C:\\Coding\\Kysect.PowerShellRunner\\Sources\\Kysect.PowerShellRunner.sln";

        _dotnetFormatCli.Format(pathToSln, "sample.json");
    }
}