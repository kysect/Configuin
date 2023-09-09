using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public interface IMsLearnDocumentationInfoProvider
{
    MsLearnDocumentationRawInfo Provide();
}