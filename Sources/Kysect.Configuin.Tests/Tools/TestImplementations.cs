using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation;

namespace Kysect.Configuin.Tests.Tools;

public static class TestImplementations
{
    public static MsLearnDocumentationInfoLocalProvider CreateDocumentationInfoLocalProvider()
    {
        return new MsLearnDocumentationInfoLocalProvider(Constants.GetPathToMsDocsRoot());
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