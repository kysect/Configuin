using Kysect.CommonLib.Logging;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Kysect.Configuin.Core.CliExecution;

public class CmdProcess
{
    private readonly ILogger _logger;

    public CmdProcess(ILogger logger)
    {
        _logger = logger;
    }

    public CmdExecutionResult ExecuteCommand(string command)
    {

        using var process = new Process();

        ProcessStartInfo startInfo = CreateProcessStartInfo(command);

        _logger.LogTrace("Execute cmd command {command} {arguments}", startInfo.FileName, startInfo.Arguments);

        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        int exitCode = process.ExitCode;
        IReadOnlyCollection<string> errors = GetErrors(process);
        var cmdExecutionResult = new CmdExecutionResult(exitCode, errors);

        if (cmdExecutionResult.IsAnyError())
        {
            _logger.LogError("Finished with {exitCode} and {errorCount} errors.", exitCode, errors.Count);
            foreach (string error in cmdExecutionResult.Errors)
                _logger.LogTabError(1, error);
        }

        process.Close();

        return cmdExecutionResult;
    }

    private ProcessStartInfo CreateProcessStartInfo(string command)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                FileName = "cmd.exe",
                Arguments = $"/C {command}"
            };
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                FileName = "sh",
                Arguments = $"-c {command}"
            };
        }

        throw new NotSupportedException(RuntimeInformation.OSDescription);
    }

    private IReadOnlyCollection<string> GetErrors(Process process)
    {
        var errors = new List<string>();

        // TODO: fixed error stream reading
        // Line splitting triggered by char limit =_=
        while (!process.StandardError.EndOfStream)
        {
            string? line = process.StandardError.ReadLine();
            if (line is not null)
                errors.Add(line);
        }

        return errors;
    }
}