using System.Diagnostics;

namespace Kysect.Configuin.Core.CliExecution;

public class CmdProcess
{
    public CmdExecutionResult ExecuteCommand(string command)
    {
        using var process = new Process();

        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardError = true,
            FileName = "cmd.exe",
            Arguments = $"/C {command}"
        };

        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        int exitCode = process.ExitCode;
        IReadOnlyCollection<string> errors = GetErrors(process);
        process.Close();

        return new CmdExecutionResult(exitCode, errors);
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