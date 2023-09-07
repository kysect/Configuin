using FluentAssertions;
using Kysect.Configuin.Core.CliExecution;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class CmdProcessTests
{
    private readonly CmdProcess _cmdProcess;

    public CmdProcessTests()
    {
        _cmdProcess = new CmdProcess(TestLogger.ProviderForTests());
    }

    [Test]
    public void Execute_ForInvalidCommand_ThrowError()
    {
        CmdExecutionResult cmdExecutionResult = _cmdProcess.ExecuteCommand("qwerasdf1234");

        cmdExecutionResult.ExitCode.Should().NotBe(0);
    }
}