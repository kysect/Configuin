using EditorConfigEditor.Core.MicrosoftLearnDocumentation.Models;

namespace EditorConfigEditor.Core.MicrosoftLearnDocumentation;

public interface IMicrosoftLearnDocumentationInfoProvider
{
    MicrosoftLearnDocumentationRawInfo Provide();
}