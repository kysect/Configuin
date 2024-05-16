using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.Tests.EditorConfig.Tools;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigDocumentNodeRewriterTests
{
    private readonly EditorConfigDocumentComparator _comparator = new EditorConfigDocumentComparator();

    [Fact]
    public void Remove_Child_ReturnDocumentWithoutChild()
    {
        EditorConfigDocument input = new EditorConfigDocument()
            .AddChild(new EditorConfigPropertyNode("first", "value"))
            .AddChild(new EditorConfigPropertyNode("second", "value"));

        EditorConfigDocument expected = new EditorConfigDocument()
            .AddChild(new EditorConfigPropertyNode("second", "value"));

        var nodeForRemoving = input.Children.First();
        EditorConfigDocument actual = input.RemoveNodes([nodeForRemoving]);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void Remove_MultipleChild_ReturnDocumentWithoutChild()
    {
        EditorConfigDocument input = new EditorConfigDocument()
            .AddChild(new EditorConfigPropertyNode("first", "value"))
            .AddChild(new EditorConfigPropertyNode("second", "value"))
            .AddChild(new EditorConfigPropertyNode("third", "value"));

        EditorConfigDocument expected = new EditorConfigDocument()
            .AddChild(new EditorConfigPropertyNode("third", "value"));

        var nodesForRemoving = input.Children.Take(2).ToList();
        EditorConfigDocument actual = input.RemoveNodes(nodesForRemoving);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void ReplaceNode_NodeWithChild_ReplaceAll()
    {
        EditorConfigDocument input = new EditorConfigDocument()
            .AddChild(new EditorConfigDocumentSectionNode("### Section ###")
                .AddChild(new EditorConfigPropertyNode("first", "value")));

        EditorConfigDocument expected = new EditorConfigDocument()
            .AddChild(new EditorConfigDocumentSectionNode("### Section ###")
                .AddChild(new EditorConfigPropertyNode("second", "value2")));

        var nodesForRemoving = input.Children.First().To<IEditorConfigContainerNode>().Children.First();
        EditorConfigDocument actual = input.ReplaceNodes(nodesForRemoving, new EditorConfigPropertyNode("second", "value2"));

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void UpdateNodes_NodeWithChild_OnlyParentChanged()
    {
        EditorConfigDocument input = new EditorConfigDocument()
            .AddChild(new EditorConfigDocumentSectionNode("### Section ###")
                .AddChild(new EditorConfigPropertyNode("first", "value")));

        EditorConfigDocument expected = new EditorConfigDocument()
            .AddChild(new EditorConfigDocumentSectionNode("### Other section ###")
                .AddChild(new EditorConfigPropertyNode("first", "value")));

        EditorConfigDocument actual = input.UpdateNodes(n =>
        {
            if (n is EditorConfigDocumentSectionNode { Value: "### Section ###" } oldSection)
                return oldSection with { Value = "### Other section ###" };
            return n;
        });

        _comparator.Compare(actual, expected);
    }
}