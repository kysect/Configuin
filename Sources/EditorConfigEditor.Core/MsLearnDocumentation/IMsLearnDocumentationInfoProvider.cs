using EditorConfigEditor.Core.MsLearnDocumentation.Models;

namespace EditorConfigEditor.Core.MsLearnDocumentation;

public interface IMsLearnDocumentationInfoProvider
{
    MsLearnDocumentationRawInfo Provide();
}