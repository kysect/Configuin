namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnDocumentationFileContents
{
    public IReadOnlyCollection<string> QualityRuleFileContents { get; }
    public IReadOnlyCollection<string> StyleRuleFileContents { get; }
    public string SharpFormattingOptionsContent { get; }
    public string DotnetFormattingOptionsContent { get; }
    public string QualityRuleOptionFileContent { get; }

    public LearnDocumentationFileContents(
        IReadOnlyCollection<string> qualityRuleFileContents,
        IReadOnlyCollection<string> styleRuleFileContents,
        string sharpFormattingOptionsContent,
        string dotnetFormattingOptionsContent, string qualityRuleOptionFileContent)
    {
        ArgumentNullException.ThrowIfNull(qualityRuleFileContents);
        ArgumentNullException.ThrowIfNull(styleRuleFileContents);
        ArgumentException.ThrowIfNullOrEmpty(sharpFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(dotnetFormattingOptionsContent);
        ArgumentException.ThrowIfNullOrEmpty(qualityRuleOptionFileContent);

        QualityRuleFileContents = qualityRuleFileContents;
        StyleRuleFileContents = styleRuleFileContents;
        SharpFormattingOptionsContent = sharpFormattingOptionsContent;
        DotnetFormattingOptionsContent = dotnetFormattingOptionsContent;
        QualityRuleOptionFileContent = qualityRuleOptionFileContent;
    }
}