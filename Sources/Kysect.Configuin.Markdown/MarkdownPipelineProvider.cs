﻿using Markdig;

namespace Kysect.Configuin.Markdown;

public class MarkdownPipelineProvider
{
    public static MarkdownPipeline GetDefault()
    {
        return new MarkdownPipelineBuilder().UseAdvancedExtensions().EnableTrackTrivia().Build();
    }
}