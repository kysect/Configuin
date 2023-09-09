using FluentAssertions;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigAnalyzerTests
{
    [Test]
    public void GetMissedConfigurations_AllOptionAreMissed_ReturnAllOptions()
    {
        var editorConfigAnalyzer = new EditorConfigAnalyzer();

        var editorConfigSettings = new EditorConfigSettings(Array.Empty<IEditorConfigSetting>());
        var roslynRules = new RoslynRules(new[] { WellKnownRoslynRuleDefinitions.CA1064() }, new[] { WellKnownRoslynRuleDefinitions.IDE0040() });

        EditorConfigMissedConfiguration editorConfigMissedConfiguration = editorConfigAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        editorConfigMissedConfiguration.QualityRuleSeverity
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.CA1064().RuleId);

        editorConfigMissedConfiguration.StyleRuleSeverity
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.IDE0040().RuleId);

        editorConfigMissedConfiguration.StyleRuleOptions
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name);
    }

    [Test]
    public void GetMissedConfigurations_AllOptionExists_ReturnAllOptions()
    {
        var editorConfigAnalyzer = new EditorConfigAnalyzer();

        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().RuleId, RoslynRuleSeverity.Warning),
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.CA1064().RuleId, RoslynRuleSeverity.Warning),
            new RoslynOptionEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name, "always")
        });

        var roslynRules = new RoslynRules(new[] { WellKnownRoslynRuleDefinitions.CA1064() }, new[] { WellKnownRoslynRuleDefinitions.IDE0040() });

        EditorConfigMissedConfiguration editorConfigMissedConfiguration = editorConfigAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        editorConfigMissedConfiguration.QualityRuleSeverity.Should().BeEmpty();
        editorConfigMissedConfiguration.StyleRuleSeverity.Should().BeEmpty();
        editorConfigMissedConfiguration.StyleRuleOptions.Should().BeEmpty();
    }
}