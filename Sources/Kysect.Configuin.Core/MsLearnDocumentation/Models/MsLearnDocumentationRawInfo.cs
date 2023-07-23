namespace Kysect.Configuin.Core.MsLearnDocumentation.Models;

public class MsLearnDocumentationRawInfo
{
    public IReadOnlyCollection<string> QualityRuleInfos { get; }
    public IReadOnlyCollection<string> StyleRuleInfos { get; }
    public string SharpFormattingOptionsContent { get; }
    public string DotnetFormattingOptionsContent { get; }

    public MsLearnDocumentationRawInfo(
        IReadOnlyCollection<string> qualityRuleInfos,
        IReadOnlyCollection<string> styleRuleInfos,
        string sharpFormattingOptionsContent,
        string dotnetFormattingOptionsContent)
    {
        ArgumentNullException.ThrowIfNull(qualityRuleInfos);
        ArgumentNullException.ThrowIfNull(styleRuleInfos);
        ArgumentException.ThrowIfNullOrEmpty(sharpFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(dotnetFormattingOptionsContent);

        QualityRuleInfos = qualityRuleInfos;
        StyleRuleInfos = styleRuleInfos;
        SharpFormattingOptionsContent = sharpFormattingOptionsContent;
        DotnetFormattingOptionsContent = dotnetFormattingOptionsContent;
    }
}