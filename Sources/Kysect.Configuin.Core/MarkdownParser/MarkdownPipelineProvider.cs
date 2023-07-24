using Markdig;

namespace Kysect.Configuin.Core.MarkdownParser;

public class MarkdownPipelineProvider
{
    public static MarkdownPipeline GetDefault()
    {
        return new MarkdownPipelineBuilder().UseAdvancedExtensions().EnableTrackTrivia().Build();
    }
}