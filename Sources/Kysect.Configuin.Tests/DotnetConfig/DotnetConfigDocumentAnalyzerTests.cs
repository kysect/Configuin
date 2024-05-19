using Kysect.Configuin.DotnetConfig.Analyzing;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Resources;

namespace Kysect.Configuin.Tests.DotnetConfig;

public class DotnetConfigDocumentAnalyzerTests
{
    private readonly DotnetConfigDocumentAnalyzer _dotnetConfigDocumentAnalyzer;

    public DotnetConfigDocumentAnalyzerTests()
    {
        _dotnetConfigDocumentAnalyzer = new DotnetConfigDocumentAnalyzer();
    }

    [Fact]
    public void GetMissedConfigurations_AllOptionAreMissed_ReturnAllOptions()
    {
        var editorConfigSettings = new DotnetConfigDocument();
        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        DotnetConfigMissedConfiguration dotnetConfigMissedConfiguration = _dotnetConfigDocumentAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        dotnetConfigMissedConfiguration.QualityRuleSeverity
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.CA1064().RuleId);

        dotnetConfigMissedConfiguration.StyleRuleSeverity
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.IDE0040().Rules.Single().RuleId);

        dotnetConfigMissedConfiguration.StyleRuleOptions
            .Should().HaveCount(1)
            .And.Contain(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name);
    }

    [Fact]
    public void GetMissedConfigurations_AllOptionExists_ReturnAllOptions()
    {
        var editorConfigSettings = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleSeverityNode(WellKnownRoslynRuleDefinitions.IDE0040().Rules.Single().RuleId, RoslynRuleSeverity.Warning.ToString()))
            .AddChild(new DotnetConfigRuleSeverityNode(WellKnownRoslynRuleDefinitions.CA1064().RuleId, RoslynRuleSeverity.Warning.ToString()))
            .AddChild(new DotnetConfigRuleOptionNode(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name, "always"));

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        DotnetConfigMissedConfiguration dotnetConfigMissedConfiguration = _dotnetConfigDocumentAnalyzer.GetMissedConfigurations(editorConfigSettings, roslynRules);

        dotnetConfigMissedConfiguration.QualityRuleSeverity.Should().BeEmpty();
        dotnetConfigMissedConfiguration.StyleRuleSeverity.Should().BeEmpty();
        dotnetConfigMissedConfiguration.StyleRuleOptions.Should().BeEmpty();
    }

    [Fact]
    public void GetIncorrectOptionValues_AllOptionsValid_NoElementReturn()
    {
        var editorConfigSettings = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode(WellKnownRoslynRuleDefinitions.IDE0040().Options.Single().Name, "always"));

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        IReadOnlyCollection<DotnetConfigInvalidOptionValue> invalidOptionValues = _dotnetConfigDocumentAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().BeEmpty();
    }

    [Fact]
    public void GetIncorrectOptionValues_ForInvalidOptionValue_ReturnInvalidOption()
    {
        string incorrectOptionValue = "null";
        RoslynStyleRuleOption selectedOption = WellKnownRoslynRuleDefinitions.IDE0040().Options.Single();
        var editorConfigSettings = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode(selectedOption.Name, incorrectOptionValue));

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        var expected = new DotnetConfigInvalidOptionValue(
            selectedOption.Name,
            incorrectOptionValue,
            selectedOption.Values);

        IReadOnlyCollection<DotnetConfigInvalidOptionValue> invalidOptionValues = _dotnetConfigDocumentAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetIncorrectOptionValues_ForInvalidOptionKey_ReturnInvalidOption()
    {
        string incorrectOptionKey = "null";
        string incorrectOptionValue = "null";

        var editorConfigSettings = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleOptionNode(incorrectOptionKey, incorrectOptionValue));

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddStyle(WellKnownRoslynRuleDefinitions.IDE0040())
            .Build();

        var expected = new DotnetConfigInvalidOptionValue(
            incorrectOptionKey,
            incorrectOptionValue,
            Array.Empty<RoslynStyleRuleOptionValue>());

        IReadOnlyCollection<DotnetConfigInvalidOptionValue> invalidOptionValues = _dotnetConfigDocumentAnalyzer.GetIncorrectOptionValues(editorConfigSettings, roslynRules);

        invalidOptionValues.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetIncorrectOptionSeverity_ForInvalidSeverityConfiguration_ReturnInvalidRuleIds()
    {
        var editorConfigSettings = new DotnetConfigDocument()
            .AddChild(new DotnetConfigRuleSeverityNode(WellKnownRoslynRuleDefinitions.IDE0040().Rules.Single().RuleId, RoslynRuleSeverity.Warning.ToString()))
            .AddChild(new DotnetConfigRuleSeverityNode(WellKnownRoslynRuleDefinitions.CA1064().RuleId, RoslynRuleSeverity.Warning.ToString()));

        RoslynRules roslynRules = RoslynRulesBuilder.New()
            .AddQuality(WellKnownRoslynRuleDefinitions.CA1064())
            .Build();

        IReadOnlyCollection<RoslynRuleId> incorrectOptionSeverity = _dotnetConfigDocumentAnalyzer.GetIncorrectOptionSeverity(editorConfigSettings, roslynRules);

        incorrectOptionSeverity.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(WellKnownRoslynRuleDefinitions.IDE0040().Rules.Single().RuleId);
    }
}