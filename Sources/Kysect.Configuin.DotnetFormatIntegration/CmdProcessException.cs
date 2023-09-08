namespace Kysect.Configuin.DotnetFormatIntegration;

public class CmdProcessException : Exception
{
    public CmdProcessException(string message) : base(message)
    {
    }

    public CmdProcessException() : base("Failed to execute cmd command")
    {
    }
}