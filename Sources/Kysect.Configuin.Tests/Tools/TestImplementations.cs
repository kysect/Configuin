using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.MsLearn;

namespace Kysect.Configuin.Tests.Tools;

public static class TestImplementations
{
    public static MsLearnDocumentationInfoLocalReader CreateDocumentationInfoLocalProvider()
    {
        return new MsLearnDocumentationInfoLocalReader();
    }

    public static MsLearnRepositoryPathProvider CreateRepositoryPathProvider()
    {
        return new MsLearnRepositoryPathProvider(Constants.GetPathToMsDocsRoot());
    }

    public static IMarkdownTextExtractor GetTextExtractor()
    {
        return PlainTextExtractor.Create();
    }
}