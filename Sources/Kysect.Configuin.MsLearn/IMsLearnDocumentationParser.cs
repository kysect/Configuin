using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.MsLearn;

public interface IMsLearnDocumentationParser
{
    RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo);
}