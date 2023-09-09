using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration;

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

    public void Format(string pathToSolution, string pathToJson)
    {
        _logger.LogInformation("Generate warnings for {pathToSolution} and write result to {pathToJson}", pathToSolution, pathToJson);
        // TODO: handle exceptions in some way?
        _cmdProcess.ExecuteCommand($"dotnet format \"{pathToSolution}\" --verify-no-changes --report \"{pathToJson}\"");
    }
}