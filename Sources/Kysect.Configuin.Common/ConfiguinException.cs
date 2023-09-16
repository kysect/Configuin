namespace Kysect.Configuin.Common;

public class ConfiguinException : Exception
{
    public ConfiguinException(string message) : base(message)
    {
    }

    public ConfiguinException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ConfiguinException()
    {
    }
}