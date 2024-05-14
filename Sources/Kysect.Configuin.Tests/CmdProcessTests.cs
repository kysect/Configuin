using Kysect.Configuin.DotnetFormatIntegration.Cli;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests;

public class CmdProcessTests
{
    private readonly CmdProcess _cmdProcess;

    public CmdProcessTests()
    {
        _cmdProcess = new CmdProcess(TestLogger.ProviderForTests());
    }

    [Fact]
    public void Execute_ForInvalidCommand_ThrowError()
    {
        CmdExecutionResult cmdExecutionResult = _cmdProcess.ExecuteCommand("qwerasdf1234");

        cmdExecutionResult.ExitCode.Should().NotBe(0);
    }
}