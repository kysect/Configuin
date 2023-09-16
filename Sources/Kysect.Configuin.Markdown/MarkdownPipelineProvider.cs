using Markdig;

namespace Kysect.Configuin.Markdown;

public static class MarkdownPipelineProvider
{
    public static MarkdownPipeline GetDefault()
    {
        return new MarkdownPipelineBuilder().UseAdvancedExtensions().EnableTrackTrivia().Build();
    }
}