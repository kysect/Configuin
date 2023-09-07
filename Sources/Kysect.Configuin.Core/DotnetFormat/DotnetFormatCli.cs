using Kysect.Configuin.Core.CliExecution;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.Core.DotnetFormat;

public class DotnetFormatCli
{
    private readonly ILogger _logger;
    private readonly CmdProcess _cmdProcess;

    public DotnetFormatCli(ILogger logger)
    {
        _cmdProcess = new CmdProcess(logger);
        _logger = logger;
    }

    public void Validate()
    {
        _logger.LogInformation("Try to use dotnet format");
        _cmdProcess.ExecuteCommand("dotnet format -h").ThrowIfAnyError();
    }

    public void GenerateWarnings(string pathToSolution, string pathToJson)
    {
        _logger.LogInformation("Generate warnings for {pathToSolution} and write result to {pathToJson}", pathToSolution, pathToJson);
        _cmdProcess.ExecuteCommand($"dotnet format \"{pathToSolution}\" --verify-no-changes --report \"{pathToJson}\"").ThrowIfAnyError();
    }
}