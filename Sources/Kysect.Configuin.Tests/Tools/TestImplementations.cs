using Kysect.Configuin.Learn;
using Kysect.Configuin.Markdown.TextExtractor;

namespace Kysect.Configuin.Tests.Tools;

public static class TestImplementations
{
    public static LearnDocumentationReader CreateDocumentationInfoLocalProvider()
    {
        return new LearnDocumentationReader();
    }

    public static LearnRepositoryPathProvider CreateRepositoryPathProvider()
    {
        return new LearnRepositoryPathProvider(Constants.GetPathToMsDocsRoot());
    }

    public static IMarkdownTextExtractor GetTextExtractor()
    {
        return PlainTextExtractor.Create();
    }
}