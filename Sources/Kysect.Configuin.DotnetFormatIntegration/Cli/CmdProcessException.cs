namespace Kysect.Configuin.DotnetFormatIntegration.Cli;

public class CmdProcessException : Exception
{
    public CmdProcessException(string message) : base(message)
    {
    }

    public CmdProcessException() : base("Failed to execute cmd command")
    {
    }

    public CmdProcessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}