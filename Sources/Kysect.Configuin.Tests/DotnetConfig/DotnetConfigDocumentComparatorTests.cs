using Kysect.Configuin.DotnetConfig.Diff;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.DotnetConfig;

public class DotnetConfigDocumentComparatorTests
{
    [Fact]
    public void Compare_WithEmptyCollection_ReturnExpectedResult()
    {
        var empty = new DotnetConfigDocument();
        var left = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleSeverityNode(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning.ToString()))
            .AddChild(new DotnetConfigRuleOptionNode("OptionKey", "OptionValue"))
            .AddChild(new DotnetConfigGeneralOptionNode("Name", "Value"))
            .AddChild(new DotnetConfigRuleCompositeOptionNode(new string[] { "Key", "Parts" }, "Value"));

        var editorConfigSettingsComparator = new DotnetConfigDocumentComparator();

        DotnetConfigDocumentDiff dotnetConfigDocumentDiff = editorConfigSettingsComparator.Compare(left, empty);

        dotnetConfigDocumentDiff.SeverityDiffs
            .Should().HaveCount(1)
            .And.Contain(new DotnetConfigRuleSeverityDiff(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning, null));

        dotnetConfigDocumentDiff.OptionDiffs
            .Should().HaveCount(1)
            .And.Contain(new DotnetConfigRuleOptionDiff("OptionKey", "OptionValue", null));
    }
}