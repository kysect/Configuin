using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public interface IMsLearnDocumentationInfoReader
{
    MsLearnDocumentationRawInfo Provide(string pathToRepository);
}