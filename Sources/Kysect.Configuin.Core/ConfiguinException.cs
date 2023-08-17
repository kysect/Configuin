namespace Kysect.Configuin.Core;

public class ConfiguinException : Exception
{
    public ConfiguinException(string message) : base(message)
    {
    }

    public ConfiguinException(string message, Exception innerException) : base(message, innerException)
    {
    }
}