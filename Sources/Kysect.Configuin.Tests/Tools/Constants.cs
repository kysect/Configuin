namespace Kysect.Configuin.Tests.Tools;

public static class Constants
{
    public static string GetPathToMsDocsRoot()
    {
        return Path.Combine(
            "..", // netX.0
            "..", // Debug
            "..", // bin
            "..", // Kysect.Configuin.Tests
            "..", // root
            "ms-learn");
    }
}