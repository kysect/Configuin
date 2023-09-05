using Kysect.Configuin.Core.CliExecution;

namespace Kysect.Configuin.Core.DotnetFormat;

public class DotnetFormatCli
{
    private readonly CmdProcess _cmdProcess = new CmdProcess();

    public void Validate()
    {
        ExecuteCommand("dotnet format -h");
    }

    public void GenerateWarnings(string pathToSolution, string pathToJson)
    {

        ExecuteCommand($"dotnet format \"{pathToSolution}\" --verify-no-changes --report \"{pathToJson}\"");
    }

    private void ExecuteCommand(string command)
    {
        _cmdProcess.ExecuteCommand(command).Wait();
    }
}