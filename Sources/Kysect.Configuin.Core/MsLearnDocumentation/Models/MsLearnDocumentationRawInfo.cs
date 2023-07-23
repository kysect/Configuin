namespace Kysect.Configuin.Core.MsLearnDocumentation.Models;

public class MsLearnDocumentationRawInfo
{
    public IReadOnlyCollection<string> QualityRuleInfos { get; }
    public IReadOnlyCollection<string> StyleRuleInfos { get; }

    public MsLearnDocumentationRawInfo(IReadOnlyCollection<string> qualityRuleInfos, IReadOnlyCollection<string> styleRuleInfos)
    {
        QualityRuleInfos = qualityRuleInfos;
        StyleRuleInfos = styleRuleInfos;
    }
}