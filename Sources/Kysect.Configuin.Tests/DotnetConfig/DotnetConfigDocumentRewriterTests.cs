using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.Tests.DotnetConfig.Tools;

namespace Kysect.Configuin.Tests.DotnetConfig;

public class DotnetConfigDocumentRewriterTests
{
    private readonly DotnetConfigDocumentComparator _comparator = new DotnetConfigDocumentComparator();

    [Fact]
    public void Remove_Child_ReturnDocumentWithoutChild()
    {
        DotnetConfigDocument input = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode("first", "value"))
            .AddChild(new DotnetConfigRuleOptionNode("second", "value"));

        DotnetConfigDocument expected = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode("second", "value"));

        var nodeForRemoving = input.Children.First();
        DotnetConfigDocument actual = input.RemoveNodes([nodeForRemoving]);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void Remove_MultipleChild_ReturnDocumentWithoutChild()
    {
        DotnetConfigDocument input = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode("first", "value"))
            .AddChild(new DotnetConfigRuleOptionNode("second", "value"))
            .AddChild(new DotnetConfigRuleOptionNode("third", "value"));

        DotnetConfigDocument expected = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode("third", "value"));

        var nodesForRemoving = input.Children.Take(2).ToList();
        DotnetConfigDocument actual = input.RemoveNodes(nodesForRemoving);

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void ReplaceNode_NodeWithChild_ReplaceAll()
    {
        DotnetConfigDocument input = new DotnetConfigDocument()
            .AddChild(new DotnetConfigSectionNode("### Section ###")
                .AddChild(new DotnetConfigRuleOptionNode("first", "value")));

        DotnetConfigDocument expected = new DotnetConfigDocument()
            .AddChild(new DotnetConfigSectionNode("### Section ###")
                .AddChild(new DotnetConfigRuleOptionNode("second", "value2")));

        var nodesForRemoving = input.Children.First().To<IDotnetConfigContainerSyntaxNode>().Children.First();
        DotnetConfigDocument actual = input.ReplaceNodes(nodesForRemoving, new DotnetConfigRuleOptionNode("second", "value2"));

        _comparator.Compare(actual, expected);
    }

    [Fact]
    public void UpdateNodes_NodeWithChild_OnlyParentChanged()
    {
        DotnetConfigDocument input = new DotnetConfigDocument()
            .AddChild(new DotnetConfigSectionNode("### Section ###")
                .AddChild(new DotnetConfigRuleOptionNode("first", "value")));

        DotnetConfigDocument expected = new DotnetConfigDocument()
            .AddChild(new DotnetConfigSectionNode("### Other section ###")
                .AddChild(new DotnetConfigRuleOptionNode("first", "value")));

        DotnetConfigDocument actual = input.UpdateNodes(n =>
        {
            if (n is DotnetConfigSectionNode { Value: "### Section ###" } oldSection)
                return oldSection with { Value = "### Other section ###" };
            return n;
        });

        _comparator.Compare(actual, expected);
    }
}