using EditorConfigEditor.Core.MsLearnDocumentation.Models;
using EditorConfigEditor.Core.RoslynRuleModels;

namespace EditorConfigEditor.Core.MsLearnDocumentation;

public interface IMsLearnDocumentationParser
{
    RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo);
}