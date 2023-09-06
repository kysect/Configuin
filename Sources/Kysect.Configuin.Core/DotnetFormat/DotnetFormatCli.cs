using Kysect.Configuin.Core.CliExecution;

namespace Kysect.Configuin.Core.DotnetFormat;

public class DotnetFormatCli
{
    private readonly CmdProcess _cmdProcess = new CmdProcess();

    public void Validate()
    {
        _cmdProcess.ExecuteCommand("dotnet format -h").ThrowIfAnyError();
    }

    public void GenerateWarnings(string pathToSolution, string pathToJson)
    {

        _cmdProcess.ExecuteCommand($"dotnet format \"{pathToSolution}\" --verify-no-changes --report \"{pathToJson}\"").ThrowIfAnyError();
    }
}