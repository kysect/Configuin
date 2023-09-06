using FluentAssertions;
using Kysect.Configuin.Core.CliExecution;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class CmdProcessTests
{
    [Test]
    public void Execute_ForInvalidCommand_ThrowError()
    {
        var cmdProcess = new CmdProcess();

        CmdExecutionResult cmdExecutionResult = cmdProcess.ExecuteCommand("qwerasdf1234");

        cmdExecutionResult.ExitCode.Should().NotBe(0);
    }
}