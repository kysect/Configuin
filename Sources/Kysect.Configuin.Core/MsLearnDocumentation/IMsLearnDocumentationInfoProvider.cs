using Kysect.Configuin.Core.MsLearnDocumentation.Models;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public interface IMsLearnDocumentationInfoProvider
{
    MsLearnDocumentationRawInfo Provide();
}