using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public class EditorConfigDocumentSerializer
{
    public string Build(EditorConfigDocument document)
    {
        document.ThrowIfNull();

        return document.ToFullString();
    }
}