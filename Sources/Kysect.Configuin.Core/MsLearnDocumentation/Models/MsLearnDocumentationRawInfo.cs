namespace Kysect.Configuin.Core.MsLearnDocumentation.Models;

public class MsLearnDocumentationRawInfo
{
    public IReadOnlyCollection<string> QualityRuleFileContents { get; }
    public IReadOnlyCollection<string> StyleRuleFileContents { get; }
    public string SharpFormattingOptionsContent { get; }
    public string DotnetFormattingOptionsContent { get; }

    public MsLearnDocumentationRawInfo(
        IReadOnlyCollection<string> qualityRuleFileContents,
        IReadOnlyCollection<string> styleRuleFileContents,
        string sharpFormattingOptionsContent,
        string dotnetFormattingOptionsContent)
    {
        ArgumentNullException.ThrowIfNull(qualityRuleFileContents);
        ArgumentNullException.ThrowIfNull(styleRuleFileContents);
        ArgumentException.ThrowIfNullOrEmpty(sharpFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(dotnetFormattingOptionsContent);

        QualityRuleFileContents = qualityRuleFileContents;
        StyleRuleFileContents = styleRuleFileContents;
        SharpFormattingOptionsContent = sharpFormattingOptionsContent;
        DotnetFormattingOptionsContent = dotnetFormattingOptionsContent;
    }
}