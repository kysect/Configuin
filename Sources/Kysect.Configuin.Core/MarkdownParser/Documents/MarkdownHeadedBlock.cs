﻿using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParser.Documents;

public class MarkdownHeadedBlock
{
    public HeadingBlock? Header { get; }
    public IReadOnlyCollection<Block> Content { get; }

    public MarkdownHeadedBlock(HeadingBlock? header, IReadOnlyCollection<Block> content)
    {
        Header = header;
        Content = content;
    }
}