using Kysect.Configuin.Core.CliExecution;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class CmdProcessTests
{
    [Test]
    public void Execute_ForInvalidCommand_ThrowError()
    {
        var cmdProcess = new CmdProcess();

        Assert.Throws<AggregateException>(() =>
        {
            cmdProcess.ExecuteCommand("qwerasdf1234").Wait();
        });
    }
}