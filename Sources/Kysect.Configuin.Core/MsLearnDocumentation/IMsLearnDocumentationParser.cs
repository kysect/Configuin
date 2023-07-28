using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public interface IMsLearnDocumentationParser
{
    RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo);
    RoslynStyleRule ParseStyleRule(string info);
}