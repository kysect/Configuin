using System.Collections.Immutable;
using System.Text;

namespace Kysect.Configuin.EditorConfig.DocumentModel.Nodes;

public record EditorConfigDocument(ImmutableList<IEditorConfigNode> Children, ImmutableList<string> TrailingTrivia) : IEditorConfigContainerNode
{
    public EditorConfigDocument(ImmutableList<IEditorConfigNode> children) : this(children, ImmutableList<string>.Empty)
    {
    }

    IEditorConfigContainerNode IEditorConfigContainerNode.AddChild(IEditorConfigNode child)
    {
        return AddChild(child);
    }

    public EditorConfigDocument AddChild(IEditorConfigNode child)
    {
        return this with { Children = Children.Add(child) };
    }

    public string ToFullString()
    {
        var stringBuilder = new StringBuilder();
        List<string> lines = new();
        lines.AddRange(Children.Select(c => c.ToFullString()));
        lines.AddRange(TrailingTrivia);

        stringBuilder.AppendJoin(Environment.NewLine, lines);
        return stringBuilder.ToString();
    }
}