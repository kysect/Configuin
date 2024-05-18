namespace Kysect.Configuin.MsLearn.Models;

public class MsLearnDocumentationRawInfo
{
    public IReadOnlyCollection<string> QualityRuleFileContents { get; }
    public IReadOnlyCollection<string> StyleRuleFileContents { get; }
    public string SharpFormattingOptionsContent { get; }
    public string DotnetFormattingOptionsContent { get; }
    public string QualityRuleOptions { get; }

    public MsLearnDocumentationRawInfo(
        IReadOnlyCollection<string> qualityRuleFileContents,
        IReadOnlyCollection<string> styleRuleFileContents,
        string sharpFormattingOptionsContent,
        string dotnetFormattingOptionsContent, string qualityRuleOptions)
    {
        ArgumentNullException.ThrowIfNull(qualityRuleFileContents);
        ArgumentNullException.ThrowIfNull(styleRuleFileContents);
        ArgumentException.ThrowIfNullOrEmpty(sharpFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(dotnetFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(qualityRuleOptions);

        QualityRuleFileContents = qualityRuleFileContents;
        StyleRuleFileContents = styleRuleFileContents;
        SharpFormattingOptionsContent = sharpFormattingOptionsContent;
        DotnetFormattingOptionsContent = dotnetFormattingOptionsContent;
        QualityRuleOptions = qualityRuleOptions;
    }
}