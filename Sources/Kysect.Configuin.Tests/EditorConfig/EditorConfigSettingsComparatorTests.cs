using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Diff;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigSettingsComparatorTests
{
    [Fact]
    public void Compare_WithEmptyCollection_ReturnExpectedResult()
    {
        var empty = new DotnetConfigSettings(Array.Empty<IEditorConfigSetting>());
        var editorConfigSettings = new DotnetConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynSeverityEditorConfigSetting(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning),
            // TODO: use real option?
            new RoslynOptionEditorConfigSetting("OptionKey", "OptionValue"),
            new GeneralEditorConfigSetting("Name", "Value"),
            new CompositeRoslynOptionEditorConfigSetting(new string[] {"Key", "Parts"}, "Value", RoslynRuleSeverity.Warning)
        });

        var editorConfigSettingsComparator = new EditorConfigSettingsComparator();

        EditorConfigSettingsDiff editorConfigSettingsDiff = editorConfigSettingsComparator.Compare(editorConfigSettings, empty);

        editorConfigSettingsDiff.SeverityDiffs
            .Should().HaveCount(1)
            .And.Contain(new EditorConfigSettingsRuleSeverityDiff(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning, null));

        editorConfigSettingsDiff.OptionDiffs
            .Should().HaveCount(1)
            .And.Contain(new EditorConfigSettingsRuleOptionDiff("OptionKey", "OptionValue", null));
    }
}