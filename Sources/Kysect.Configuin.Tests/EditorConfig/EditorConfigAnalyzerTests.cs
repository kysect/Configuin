using FluentAssertions;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.Analyzing;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfig;

public class EditorConfigAnalyzerTests
{
    private readonly EditorConfigAnalyzer _editorConfigAnalyzer;

    public EditorConfigAnalyzerTests()
    {
        _editorConfigAnalyzer = new EditorConfigAnalyzer();
    }

    [Test]
    public void GetMissedConfigurations_AllOptionAreMissed_ReturnAllOptions()
    {
        var editorConfigSettings = new EditorConfigSettings(Array.Empty<IEditorConfigSetting>());
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();
        
        EditorConfigMissedConfiguration editorConfigMissedConfiguration = _editorConfigAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

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
        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().RuleId, RoslynRuleSeverity.Warning),
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.CA1064().RuleId, RoslynRuleSeverity.Warning),
            new RoslynOptionEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name, "always")
        });

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        EditorConfigMissedConfiguration editorConfigMissedConfiguration = _editorConfigAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        editorConfigMissedConfiguration.QualityRuleSeverity.Should().BeEmpty();
        editorConfigMissedConfiguration.StyleRuleSeverity.Should().BeEmpty();
        editorConfigMissedConfiguration.StyleRuleOptions.Should().BeEmpty();
    }

    [Test]
    public void GetIncorrectOptionValues_AllOptionsValid_NoElementReturn()
    {
        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynOptionEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name, "always")
        });

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();
        
        IReadOnlyCollection<EditorConfigInvalidOptionValue> invalidOptionValues = _editorConfigAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().BeEmpty();
    }

    [Test]
    public void GetIncorrectOptionValues_ForInvalidOptionValue_ReturnInvalidOption()
    {
        string incorrectOptionValue = "null";
        RoslynStyleRuleOption selectedOptions = WellKnownRoslynRuleDefinitions.IDE0040().Options.Single();

        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynOptionEditorConfigSetting(selectedOptions.Name, incorrectOptionValue)
        });

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();
        
        var expected = new EditorConfigInvalidOptionValue(
            selectedOptions.Name,
            incorrectOptionValue,
            selectedOptions.Values);

        IReadOnlyCollection<EditorConfigInvalidOptionValue> invalidOptionValues = _editorConfigAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetIncorrectOptionValues_ForInvalidOptionKey_ReturnInvalidOption()
    {
        string incorrectOptionKey = "null";
        string incorrectOptionValue = "null";

        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynOptionEditorConfigSetting(incorrectOptionKey, incorrectOptionValue)
        });

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        var expected = new EditorConfigInvalidOptionValue(
            incorrectOptionKey,
            incorrectOptionValue,
            Array.Empty<RoslynStyleRuleOptionValue>());

        IReadOnlyCollection<EditorConfigInvalidOptionValue> invalidOptionValues = _editorConfigAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetIncorrectOptionSeverity_ForInvalidSeverityConfiguration_ReturnInvalidRuleIds()
    {
        var editorConfigSettings = new EditorConfigSettings(new IEditorConfigSetting[]
        {
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.IDE0040().RuleId, RoslynRuleSeverity.Warning),
            new RoslynSeverityEditorConfigSetting(WellKnownRoslynRuleDefinitions.CA1064().RuleId, RoslynRuleSeverity.Warning),
        });

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .Build();

        IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity = _editorConfigAnalyzer.GetIncorrectOptionSeverity(editorConfigSettings, roslynRules);

        incorrectOptionSeverity.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleDefinitions.IDE0040().RuleId);
    }
}