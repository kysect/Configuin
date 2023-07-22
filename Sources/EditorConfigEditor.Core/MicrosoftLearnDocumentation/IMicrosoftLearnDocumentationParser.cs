using EditorConfigEditor.Core.EditorConfigModels;
using EditorConfigEditor.Core.MicrosoftLearnDocumentation.Models;

namespace EditorConfigEditor.Core.MicrosoftLearnDocumentation;

public interface IMicrosoftLearnDocumentationParser
{
    RoslynRules Parse(MicrosoftLearnDocumentationRawInfo rawInfo);
}