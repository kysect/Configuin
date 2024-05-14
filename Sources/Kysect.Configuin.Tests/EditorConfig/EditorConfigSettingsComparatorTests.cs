using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Diff;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigSettingsComparatorTests
{
    public void Compare_WithEmptyCollection_ReturnExpectedResult()
    {
        var empty = new EditorConfigSettings(Array.Empty<IEditorConfigSetting>());
        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynSeverityEditorConfigSetting(RoslynRuleId.Parse("IDE0001"), RoslynRuleSeverity.Warning),
            // TODO: use real option?
            new RoslynOptionEditorConfigSetting("Value", "Key"),
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
            .And.Contain(new EditorConfigSettingsRuleOptionDiff("Name", "Value", null));
    }
}