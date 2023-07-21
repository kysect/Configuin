namespace EditorConfigEditor.Core.MicrosoftLearnDocumentation.Models;

public class MicrosoftLearnDocumentationRawInfo
{
    public IReadOnlyCollection<string> QualityRuleInfo { get; }
    public IReadOnlyCollection<string> StyleRuleInfo { get; }

    public MicrosoftLearnDocumentationRawInfo(IReadOnlyCollection<string> qualityRuleInfo, IReadOnlyCollection<string> styleRuleInfo)
    {
        QualityRuleInfo = qualityRuleInfo;
        StyleRuleInfo = styleRuleInfo;
    }
}