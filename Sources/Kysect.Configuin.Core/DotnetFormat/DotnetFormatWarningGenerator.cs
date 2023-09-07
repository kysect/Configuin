using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kysect.Configuin.Core.DotnetFormat;

public class DotnetFormatWarningGenerator
{
    private readonly ILogger _logger;
    private readonly DotnetFormatCli _dotnetFormatCli;

    public DotnetFormatWarningGenerator(ILogger logger)
    {
        _logger = logger;

        _dotnetFormatCli = new DotnetFormatCli(logger);
    }

    public IReadOnlyCollection<DotnetFormatFileReport> GenerateWarnings(string pathToSolution)
    {
        string filePath = $"output-{Guid.NewGuid()}.json";
        _logger.LogInformation("Generate dotnet format warnings for {path} will save to {output}", pathToSolution, filePath);
        _dotnetFormatCli.Format(pathToSolution, filePath);
        string warningFileContent = File.ReadAllText(filePath);
        _logger.LogInformation("Remove temp file {path}", filePath);
        File.Delete(filePath);
        return JsonSerializer.Deserialize<IReadOnlyCollection<DotnetFormatFileReport>>(warningFileContent).ThrowIfNull();
    }
}