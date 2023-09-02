using Markdig.Syntax;

namespace Kysect.Configuin.Core.MarkdownParsing.Documents;

public class MarkdownHeadedBlock
{
    public string HeaderText { get; }
    public IReadOnlyCollection<Block> Content { get; }

    public MarkdownHeadedBlock(string headerText, IReadOnlyCollection<Block> content)
    {
        HeaderText = headerText;
        Content = content;
    }
}