using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Tests.Resources;

public static class WellKnownRoslynRuleDefinitions
{
    public static RoslynStyleRule IDE0040()
    {
        var codeSample = """
                         // dotnet_style_require_accessibility_modifiers = always
                         // dotnet_style_require_accessibility_modifiers = for_non_interface_members
                         class MyClass
                         {
                             private const string thisFieldIsConst = "constant";
                         }

                         // dotnet_style_require_accessibility_modifiers = never
                         class MyClass
                         {
                             const string thisFieldIsConst = "constant";
                         }
                         """;

        RoslynStyleRuleOptionValue[] expectedOptionValues =
        {
            new RoslynStyleRuleOptionValue("always", "Prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("for_non_interface_members", "Prefer accessibility modifiers except for public interface members."),
            new RoslynStyleRuleOptionValue("never", "Do not prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("omit_if_default", "Prefer accessibility modifiers except if they are the default modifier.")
        };

        var expectedOption = new RoslynStyleRuleOption(
            Name: "dotnet_style_require_accessibility_modifiers",
            expectedOptionValues,
            DefaultValue: "for_non_interface_members",
            codeSample);

        return new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0040"),
            "Add accessibility modifiers",
            "Style",
            "This style rule concerns requiring accessibility modifiers in declarations.",
            string.Empty,
            new[] { expectedOption });
    }

    public static RoslynQualityRule CA1064()
    {
        // TODO: parse description
        return new RoslynQualityRule(
            RoslynRuleId.Parse("CA1064"),
            string.Empty,
            "Design",
            string.Empty);
    }
}