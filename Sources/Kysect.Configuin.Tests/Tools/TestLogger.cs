using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kysect.Configuin.Tests.Tools;

public static class TestLogger
{
    public static ILogger ProviderForTests()
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console();

        using var factory = new LoggerFactory();

        return factory
            .DemystifyExceptions()
            .AddSerilog(loggerConfiguration.CreateLogger())
            .CreateLogger("Tests");
    }
}