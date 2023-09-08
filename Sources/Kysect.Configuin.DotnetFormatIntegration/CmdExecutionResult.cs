namespace Kysect.Configuin.DotnetFormatIntegration;

public class CmdExecutionResult
{
    public int ExitCode { get; init; }
    public IReadOnlyCollection<string> Errors { get; init; }

    public CmdExecutionResult(int exitCode, IReadOnlyCollection<string> errors)
    {
        ExitCode = exitCode;
        Errors = errors;
    }

    public void ThrowIfAnyError()
    {
        if (Errors.Count == 1)
            throw new CmdProcessException(Errors.Single());

        if (Errors.Count > 0)
        {
            var exceptions = Errors
                .Select(m => new CmdProcessException(m))
                .ToList();
            throw new AggregateException(exceptions);
        }

        if (ExitCode != 0)
            throw new CmdProcessException($"Return {ExitCode} exit code.");
    }

    public bool IsAnyError()
    {
        return Errors.Any() || ExitCode != 0;
    }
}