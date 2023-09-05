using Kysect.CommonLib.BaseTypes.Extensions;
using System.Diagnostics;

namespace Kysect.Configuin.Core.CliExecution;

public class CmdProcess
{
    public async Task ExecuteCommand(string command)
    {
        var exceptions = new List<Exception>();

        using var process = new Process();

        process.Exited += (sender, _) =>
        {
            if (sender is null)
            {
                exceptions.Add(new NullReferenceException(nameof(sender)));
                return;
            }

            int exitCode = sender.To<Process>().ExitCode;
            if (exitCode != 0)
            {
                exceptions.Add(new CmdProcessException($"Application return exit code {exitCode}."));
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data is not null)
                exceptions.Add(new CmdProcessException());
        };

        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardError = true,
            FileName = "cmd.exe",
            Arguments = $"/C {command}"
        };

        process.StartInfo = startInfo;
        process.Start();
        await process.WaitForExitAsync();

        string errors = await process.StandardError.ReadToEndAsync();
        if (!string.IsNullOrEmpty(errors))
            exceptions.Add(new CmdProcessException(errors));

        process.Close();

        if (exceptions.Count > 0)
            throw new AggregateException("Failed to execute cmd command", exceptions);
    }
}