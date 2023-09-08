using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public interface IMsLearnDocumentationParser
{
    RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo);
}