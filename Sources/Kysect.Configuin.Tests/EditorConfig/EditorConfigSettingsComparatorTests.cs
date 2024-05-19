using Kysect.Configuin.EditorConfig.Diff;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigSettingsComparatorTests
{
    [Fact]
    public void Compare_WithEmptyCollection_ReturnExpectedResult()
    {
        var empty = new EditorConfigDocument();
        var left = new EditorConfigDocument()
            .AddChild(new EditorConfigRuleSeverityNode(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning.ToString()))
            .AddChild(new EditorConfigRuleOptionNode("OptionKey", "OptionValue"))
            .AddChild(new EditorConfigGeneralOptionNode("Name", "Value"))
            .AddChild(new EditorConfigRuleCompositeOptionNode(new string[] { "Key", "Parts" }, "Value"));

        var editorConfigSettingsComparator = new EditorConfigSettingsComparator();

        EditorConfigSettingsDiff editorConfigSettingsDiff = editorConfigSettingsComparator.Compare(left, empty);

        editorConfigSettingsDiff.SeverityDiffs
            .Should().HaveCount(1)
            .And.Contain(new EditorConfigSettingsRuleSeverityDiff(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning, null));

        editorConfigSettingsDiff.OptionDiffs
            .Should().HaveCount(1)
            .And.Contain(new EditorConfigSettingsRuleOptionDiff("OptionKey", "OptionValue", null));
    }
}